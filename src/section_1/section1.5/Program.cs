using System.Diagnostics;
using StackExchange.Redis;

var options = new ConfigurationOptions
{
    EndPoints = new EndPointCollection { "localhost:6379" }
};

var muxer = ConnectionMultiplexer.Connect(options);
var db = muxer.GetDatabase();

var stopwatch = Stopwatch.StartNew();
// TODO for Coding Challenge Start here on starting-point branch

// end Challenge