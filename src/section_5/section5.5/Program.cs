using Redis.OM;
using Redis.OM.Aggregation.AggregationPredicates;
using Redis.OM.Modeling;
using section5._5;

var provider = new RedisConnectionProvider("redis://localhost:6379");

provider.Connection.DropIndexAndAssociatedRecords(typeof(Sale));
provider.Connection.DropIndexAndAssociatedRecords(typeof(Employee));

await provider.Connection.CreateIndexAsync(typeof(Sale));
await provider.Connection.CreateIndexAsync(typeof(Employee));

var employees = provider.RedisCollection<Employee>();

// Create a couple of employees.
var alice = new Employee
{
    Name = "Alice",
    Age = 45,
    Address = new Address { StreetAddress = "Elm Street", Location = new GeoLoc(-81.957, 27.058), PostalCode = "34269" },
    Sales = new List<string>(),
    SalesAdjustment = 1.1
};

var bob = new Employee
{
    Name = "Bob",
    Age = 60,
    Address = new Address() { StreetAddress = "Bleecker Street", Location = new GeoLoc(-74.003, 40.732), PostalCode = "10014" },
    Sales = new List<string>(),
    SalesAdjustment = .9
};

await employees.InsertAsync(bob);
await employees.InsertAsync(alice);

var sales = provider.RedisCollection<Sale>();
var saleInsertTasks = new List<Task<string>>();
var random = new Random();

foreach (var emp in new[] { alice,bob })
{
    for (var i = 0; i < 100; i++)
    {
        var sale = new Sale
        {
            Total = random.Next(1000, 30000),
            EmployeeId = emp.Id
        };
        saleInsertTasks.Add(sales.InsertAsync(sale));
        emp.Sales!.Add(sale.Id!);
        emp.TotalSales += sale.Total;

    }
}

await Task.WhenAll(saleInsertTasks);
await employees.UpdateAsync(alice);
await employees.UpdateAsync(bob);

var saleAggregations = provider.AggregationSet<Sale>();

var sumBobSales = saleAggregations.Filter(x=>x.RecordShell!.EmployeeId == bob.Id).Sum(x=>x.RecordShell!.Total);
Console.WriteLine($"Bob's total sales: {sumBobSales}");

var employeeAggregations = provider.AggregationSet<Employee>();
var adjustedSales = employeeAggregations.Apply(x=>Math.Ceiling(x.RecordShell.SalesAdjustment * x.RecordShell.TotalSales), "ADJUSTED_SALES");

foreach(var employee in adjustedSales)
{
    Console.WriteLine($"Adjusted Sales: {employee["ADJUSTED_SALES"]}");
}

// String functions:

var birthdayMessage = employeeAggregations.Apply(x=>$"Happy Birthday {x.RecordShell.Name} you're now {x.RecordShell.Age} ","BIRTHDAY_MESSAGE");
foreach (var employee in birthdayMessage)
{
    Console.WriteLine($"{employee["BIRTHDAY_MESSAGE"]}");
}


// Geo Distance:
var empireGeoLoc = new GeoLoc(-74.0031713,40.7484396);

var distanceFromEmpireStateBuilding = employeeAggregations
                                                                .Load(x=>new {x.RecordShell.Name})
                                                                .Apply(x=>ApplyFunctions.GeoDistance(x.RecordShell.Address.Location, empireGeoLoc),"DISTANCE_FROM_EMPIRE_STATE_BUILDING");
foreach (var res in distanceFromEmpireStateBuilding)
{
    var employee = res.Hydrate();
    Console.WriteLine($"{employee.Name} is {res["DISTANCE_FROM_EMPIRE_STATE_BUILDING"]} meters from the Empire State Building");
}

// Top Salesperson:
var topSalesId = saleAggregations
    .GroupBy(x=>x.RecordShell.EmployeeId)
    .Sum(x=>x.RecordShell.Total)
    .OrderByDescending(x=>x["Total_SUM"])
    .Take(1)
    .First()["EmployeeId"]
    .ToString();
var topSalesPerson = await employees.FindByIdAsync(topSalesId);
Console.WriteLine($"Top seller: {topSalesPerson.Name}");