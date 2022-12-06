using Redis.OM;
using Redis.OM.Modeling;
using section5._2;

var provider = new RedisConnectionProvider("redis://localhost:6379");

provider.Connection.DropIndexAndAssociatedRecords(typeof(Sale));
provider.Connection.DropIndexAndAssociatedRecords(typeof(Employee));

await provider.Connection.CreateIndexAsync(typeof(Sale));
await provider.Connection.CreateIndexAsync(typeof(Employee));

var employees = provider.RedisCollection<Employee>();

var employee = new Employee
{
    Name = "Steve",
    Address = new Address
    {
        StreetAddress = "Main Street",
        PostalCode = "34739",
        Location = new GeoLoc(-81.006, 27.872)
    },
    Sales = new List<string>()
};
var key = employees.Insert(employee);

Console.WriteLine($"Employee Id: {employee.Id}");
Console.WriteLine($"Key Name: {key}");

var sale = new Sale
{
    Id = Guid.NewGuid().ToString(),
    Address = new Address
    {
        StreetAddress = "Pinewood Ave",
        PostalCode = "10001",
        Location = new GeoLoc( -73.991, 40.753)
    },
    EmployeeId = employee.Id,
    Total = 5000,
};

key = provider.Connection.Set(sale, TimeSpan.FromMinutes(5));
Console.WriteLine($"Sale Id: {sale.Id}");
Console.WriteLine($"Key Name: {key}");