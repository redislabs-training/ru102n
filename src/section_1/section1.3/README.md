# The Interfaces of StackExchange.Redis

## IConnectionMultiplexer

We described how to get an `IConnectionMultiplexer` in section 1.2. To quickly review initialize a `ConfigurationOptions` object with relevant parameters, then pass those options into the `ConnectionMultiplexer.Connect` or `ConnectionMultiplexer.ConnectAsync` method. This bit of code is already provided for you in `Program.cs`.

## IDatabase

The `IDatabase` is the primary interactive command interface for Redis. Anything that involves interacting with one of the Redis DataStructures involves going through the `IDatabase` interface. The `IDatabase` Interface also implements `IDatabaseAsync`, essentially a mirror of the `IDatabase` with the async variants of all of the commands. To create an `IDatabase`, call `GetDatabase` the `ConnectionMultiplexer.GetDatabase()` method.

```cs
var db = muxer.GetDatabase();
```