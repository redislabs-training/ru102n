
using NRedisTimeSeries;
using NRedisTimeSeries.Commands.Enums;
using NRedisTimeSeries.DataTypes;
using StackExchange.Redis;


var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();

// TODO for Coding Challenge Start here on starting-point branch
// Delete keys.
db.KeyDelete(new RedisKey[]{"sensor", "sensor:Max", "sensor:Avg", "sensor:Min"});

Console.ReadKey();
// end coding challenge