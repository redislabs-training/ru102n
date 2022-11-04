// See https://aka.ms/new-console-template for more information


// Start Programming Challenge
using StackExchange.Redis;
Console.WriteLine("Hello Redis!");

var muxer = ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = new EndPointCollection{"localhost:6379"}
});

var db = muxer.GetDatabase();
var res = db.Ping();
Console.WriteLine($"The ping took: {res.TotalMilliseconds} ms");
//End Programming Challenge 