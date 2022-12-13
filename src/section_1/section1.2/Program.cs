using StackExchange.Redis;

var options = new ConfigurationOptions
{
    // add and update parameters as needed
    EndPoints = new EndPointCollection{"localhost:6379"}
};

// initalize a multiplexer with ConnectionMultiplexer.Connect()
var muxer = ConnectionMultiplexer.Connect(options);

// get an IDatabase here with GetDatabase
var db = muxer.GetDatabase();

// add ping here
Console.WriteLine($"ping: {db.Ping().TotalMilliseconds} ms");