using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();

// TODO for Coding Challenge Start here on starting-point branch
var scriptText = @"
    local id = redis.call('incr', @id_key)
    local key = 'key:' .. id
    redis.call('set', key, @value)
    return key
";

var script = LuaScript.Prepare(scriptText);

var key1 = db.ScriptEvaluate(script, new {id_key=(RedisKey)"autoIncrement", value="A String Value"});
var key2 = db.ScriptEvaluate(script, new {id_key=(RedisKey)"autoIncrement", value="Another String Value"});

Console.WriteLine($"Key 1: {key1}");
Console.WriteLine($"Key 2: {key2}");
//end coding challenge
