// See https://aka.ms/new-console-template for more information

using NRedisGraph;
using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();
db.KeyDelete("pets");

// TODO for Coding Challenge Start here on starting-point branch

// end coding challenge