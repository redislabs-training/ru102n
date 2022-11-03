using StackExchange.Redis;

var options = new ConfigurationOptions
{
    EndPoints = new EndPointCollection{"localhost:6379"}
};

var muxer = ConnectionMultiplexer.Connect(options);

var db = muxer.GetDatabase();

Console.WriteLine($"ping: {db.Ping().TotalMilliseconds} ms");