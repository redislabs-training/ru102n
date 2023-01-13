# Section 1.2

Welcome to Redis University RU102N section 1.2, in this module we're going to learn how to:

1. Add StackExchange.Redis to a project.
2. Import StackExchange.Redis.
3. Connect to a Redis Instance using StackExchange.Redis.
4. Ping the Redis instance from .NET.

## Add StackExchange.Redis to the project

As you can see, we have a project file here `section1.2.csproj`. We want to add StackExchange.Redis to this project. There's a number of ways you can accomplish this, [nuget enumerates many of them](https://www.nuget.org/packages/StackExchange.Redis). The simplest way however is to use the dotnet CLI's `add package` command from this directory:

```
dotnet add package StackExchange.Redis
```

## Import StackExchange.Redis

Now that we've added StackExchange.Redis to the project, we need to import it. All you need to add is the following at the top of `Program.cs`, and all the relevant classes will be available to you.

```cs
using StackExchange.Redis;
```

## Connect to Redis

Now that we've added Redis to the project and imported it, we can go about connecting to Redis. There are two ways to connect.

1. With a connection string
2. With a `ConfigurationOptions` object 

The connection string will be pretty simple if you're running locally, but so we don't add a ton of unecessary complexity let's use the `ConfigurationOptions` object. With the `ConfigurationOptions` object, you need to pass in a collection of Endpoints (the `host:port` combinations of your Redis instance), as well as whatever other options are required to connect to your instance of Redis. For instance if you are using a free-tiered instance on Redis Cloud you will need to provide the password for your Redis Instance. After your `ConfigurationOptions` object is created, you can call the static method `ConnectionMultiplexer.Connect` or you can await the static method `ConectionMultiplexer.ConnectAsync` passing in your `ConfigurationOptions` object. Either will work. When that exits you will have a connected multiplexer.

```cs
var options = new ConfigurationOptions
{
    EndPoints = new EndPointCollection{"localhost:6379"}
};

var muxer = ConnectionMultiplexer.Connect(options);
```


## Ping Redis

Now that we've connected to Redis. All that's left to do is ping it. To ping Redis, pull an instance of an `IDatabase` from the Multiplexer using `GetDatabase`, then pinging the Redis server is as simple as calling `Ping`:

```cs
var db = muxer.GetDatabase();

Console.WriteLine($"ping: {db.Ping().TotalMilliseconds} ms");
```

## Run the app

Now all that's left to do is run your app. Use `dotnet run` to run it and you'll see an output like:

```
ping: 0.5698 ms
```