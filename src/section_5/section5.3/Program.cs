// See https://aka.ms/new-console-template for more information

using Redis.OM;
using Redis.OM.Modeling;
using section5._3;

Console.WriteLine("Hello, World!");

var provider = new RedisConnectionProvider("redis://localhost:6379");

provider.Connection.DropIndexAndAssociatedRecords(typeof(Sale));
provider.Connection.DropIndexAndAssociatedRecords(typeof(Employee));

await provider.Connection.CreateIndexAsync(typeof(Sale));
await provider.Connection.CreateIndexAsync(typeof(Employee));

var employees = provider.RedisCollection<Employee>();

// create a couple of employees
var alice = new Employee
{
    Name = "Alice",
    Age = 45,
    Address = new Address { StreetAddress = "Elm Street", Location = new GeoLoc(-81.957, 27.058), PostalCode = "34269" }
};

var bob = new Employee
{
    Name = "Bob",
    Age = 60,
    Address = new Address() { StreetAddress = "Bleecker Street", Location = new GeoLoc(-74.003, 40.732), PostalCode = "10014" }
};

var charlie = new Employee
{
    Name = "Charlie",
    Age = 26,
    Address = new Address() { StreetAddress = "Ocean Boulevard", Location = new GeoLoc(-121.869, 36.604), PostalCode = "93940" }
};

var dan = new Employee
{
    Name = "Dan",
    Age = 42,
    Address = new Address() { StreetAddress = "Baker Street", Location = new GeoLoc(-0.158, 51.523), PostalCode = "NW1 6XE" }
};

var yves = new Employee
{
    Name = "Yves",
    Age = 19,
    Address = new Address() { StreetAddress = "Rue de Rivoli", Location = new GeoLoc(2.361, 48.863), PostalCode = "75003" }
};

await employees.InsertAsync(bob);
await employees.InsertAsync(alice);
await employees.InsertAsync(charlie);
await employees.InsertAsync(dan);
await employees.InsertAsync(yves);

// Query by name

Console.WriteLine($"----Employees Named Bob----");
var alsoBob = await employees.FirstAsync(x=>x.Name == "Bob");
Console.WriteLine($"Bob's age is: {alsoBob.Age} and his postal code is: {alsoBob.Address!.PostalCode}");

// Query by age 
var employeesUnderForty = employees.Where(x => x.Age < 40);

Console.WriteLine("----Employees under 40----");
await foreach (var emp in employeesUnderForty)
{
    Console.WriteLine($"{emp.Name} is {emp.Age}");
}

// Query by proximity to Philadelphia

var employeesNearPhilly = await employees.GeoFilter(x=>x.Address!.Location, -75.159, 39.963, 1500, GeoLocDistanceUnit.Miles).ToListAsync();

Console.WriteLine("----Employees near Philly----");

foreach (var emp in employeesNearPhilly)
{
    Console.WriteLine($"{emp.Name} lives in the postal code: {emp.Address!.PostalCode}");
}

var employeesByAge = await employees.OrderBy(x=>x.Age).Select(x=>x.Name!).ToListAsync();
Console.WriteLine($"In Ascending order: {string.Join(", ", employeesByAge)}");

var employeesInReverseAlphabeticalOrder = await employees.OrderByDescending(x=>x.Name).Select(x=>x.Name!).ToListAsync();
Console.WriteLine($"In Reverse Alphabetical Order: {string.Join(", ", employeesInReverseAlphabeticalOrder)}");