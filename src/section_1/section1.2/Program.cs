using StackExchange.Redis;

// TODO for Coding Challenge Start here on starting-point branch
var options = new ConfigurationOptions
{
    // add and update parameters as needed
    EndPoints = {"localhost:6379"}
};

// initalize a multiplexer with ConnectionMultiplexer.Connect()
var muxer = ConnectionMultiplexer.Connect(options);

// get an IDatabase here with GetDatabase
var db = muxer.GetDatabase();

// add ping here
Console.WriteLine($"ping: {db.Ping().TotalMilliseconds} ms");
// end programming challenge