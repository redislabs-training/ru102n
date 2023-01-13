using Redis.OM;
using section5._1;

// TODO for Coding Challenge Start here on starting-point branch
var provider = new RedisConnectionProvider("redis://localhost:6379");

provider.Connection.DropIndexAndAssociatedRecords(typeof(Sale));
provider.Connection.DropIndexAndAssociatedRecords(typeof(Employee));

await provider.Connection.CreateIndexAsync(typeof(Sale));
await provider.Connection.CreateIndexAsync(typeof(Employee));

Console.WriteLine("Created indexes.");
// end coding challenge
