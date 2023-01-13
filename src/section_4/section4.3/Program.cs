// See https://aka.ms/new-console-template for more information

using NRedisGraph;
using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();
db.KeyDelete("pets");

// TODO for Coding Challenge Start here on starting-point branch
var graph = new RedisGraph(db);


var createBobResult = await graph.QueryAsync("pets", "CREATE(:human{name:'Bob',age:32})");
await graph.QueryAsync("pets", "CREATE(:human{name:'Alice',age:30})");

Console.WriteLine($"Nodes Created:{createBobResult.Statistics.NodesCreated}");
Console.WriteLine($"Properties Set:{createBobResult.Statistics.PropertiesSet}");
Console.WriteLine($"Labels Created:{createBobResult.Statistics.LabelsAdded}");
Console.WriteLine($"Operation took:{createBobResult.Statistics.QueryInternalExecutionTime}");

await graph.QueryAsync("pets", "CREATE(:pet{name:'Honey',age:5,species:'canine',breed:'Greyhound'})");

await graph.QueryAsync("pets",
    "MATCH(a:human),(p:pet) WHERE(a.name='Bob' and p.name='Honey') CREATE (a)-[:OWNS]->(p)");
    
await graph.QueryAsync("pets",
    "MATCH(a:human),(p:pet) WHERE(a.name='Alice' and p.name='Honey') CREATE (a)-[:WALKS]->(p)");
await graph.QueryAsync("pets",
    "MATCH(a:human),(p:pet) WHERE(a.name='Bob' and p.name='Honey') CREATE (a)-[:WALKS]->(p)");
    
var matches = await graph.QueryAsync("pets", "MATCH(a:human),(p:pet) where (a)-[:OWNS]->(p) and p.name='Honey' return a");

var record = matches.First();

Console.WriteLine($"Honey's owner nodes: {record}");

matches = await graph.QueryAsync("pets", "MATCH(a:human),(p:pet) where (a)-[:WALKS]->(p) and p.name='Honey' return a");

foreach (var rec in matches)
{
    var node = (Node)rec.Values.First();
    Console.WriteLine($"{node.PropertyMap["name"].Value} walks honey");
}

matches = await graph.QueryAsync("pets", "MATCH(a:human),(p:pet) where (a)-[:OWNS]->(p) and p.species='canine' and a.name='Bob' return p");

foreach (var rec in matches)
{
    var dogs = rec.Values.Select(x=>(Node)x).Select(x=>x.PropertyMap["name"].Value.ToString());
    Console.WriteLine($"Bob's dogs are: {string.Join(", ", dogs)}");
}

// end coding challenge