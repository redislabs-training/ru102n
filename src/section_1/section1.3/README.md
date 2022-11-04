# The Interfaces of StackExchange.Redis

## IConnectionMultiplexer

We described how to get an `IConnectionMultiplexer` in section 1.2. To quickly review initialize a `ConfigurationOptions` object with relevant parameters, then pass those options into the `ConnectionMultiplexer.Connect` or `ConnectionMultiplexer.ConnectAsync` method. This bit of code is already provided for you in `Program.cs`.

## IDatabase

The `IDatabase` is the primary interactive command interface for Redis. Anything that involves interacting with one of the Redis DataStructures involves going through the `IDatabase` interface. The `IDatabase` Interface also implements `IDatabaseAsync`, essentially a mirror of the `IDatabase` with the async variants of all of the commands. To create an `IDatabase`, call `GetDatabase` the `ConnectionMultiplexer.GetDatabase()` method.

```cs
var db = muxer.GetDatabase();
```

## IServer

The `IServer` is similar to the `IDatabase` in that it is an interactive command interface for server scoped commands. Each instance of an `IServer` maps to an individual Redis server, this conceptually differs from what deem a 'Redis Instance' in that a Redis instance might be comprised of one or many Redis Servers. For example in a cluster with 3 shards, each shard is an instance of a Redis Server. Likewise a Sentinel instance with 2 replicas, the master and both replica's would be 