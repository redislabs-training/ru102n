// See https://aka.ms/new-console-template for more information

using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = new EndPointCollection{"localhost:6379"}
});

var db = muxer.GetDatabase();
