using StackExchange.Redis;
var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();

// TODO for Coding Challenge Start here on starting-point branch
// Let's start out by creating a bunch of proper keynames, and then deleting all those sets from Redis
// This will ensure that we have a clean slate to work with.
var allUsersSet = "users";
var activeUsersSet = "users:state:active";
var inactiveUsersSet = "users:state:inactive";
var offlineUsersSet = "users:state:offline";
db.KeyDelete(new RedisKey[]{allUsersSet, activeUsersSet,inactiveUsersSet, offlineUsersSet});

// Next, we'll do some SADD's (SetAdd), this will insert all the relevant users into the relevant sets:
db.SetAdd(activeUsersSet, new RedisValue[]{"User:1","User:2"});
db.SetAdd(inactiveUsersSet, new RedisValue[]{"User:3", "User:4"});
db.SetAdd(offlineUsersSet, new RedisValue[] {"User:5", "User:6", "User:7"});

// You'll notice that we never initialized the allUsersSet, well let's do that now. We'll consider the allUsersSet to
// be the union of our three other sets. Consequentially we can do SetCombineAndStore, passing in the three sets we want
// to combine, and specifying the Union operation.
db.SetCombineAndStore(SetOperation.Union, allUsersSet, new RedisKey[]{activeUsersSet, inactiveUsersSet, offlineUsersSet});

// Now, let's check to see if a particular set contains a particular member, this is as simple as calling SetContains
// passing in the set name and the user ID that we're looking for:
var user6Offline = db.SetContains(offlineUsersSet, "User:6");
Console.WriteLine($"User:6 offline: {user6Offline}");

// Next, let's look at enumerating all the members of a set. There are realistically two ways we can do this.
// The first is totally brute force, we can use the SetMembers method, which will pull back all of the members in one shot:
Console.WriteLine($"All Users In one shot    : {string.Join(", ", db.SetMembers(allUsersSet))}");

// Set members works great for sets with smaller cardinalities (number of members), say under 1000. As we hit larger sets
// however we'll start to run into performance issues where we lock up Redis while we pull back all these members. In cases like
// these, we can use SetScan to break up that processing across a series of requests.
Console.WriteLine($"All Users with scan      : {string.Join(", ", db.SetScan(allUsersSet))}");

// SetScan, can also scan using glob patterns. This will not decrease the number of iterations it needs to perform, just the number
// it has to return. This example here will bring back all the user IDs ending in 6:
Console.WriteLine($"All Users ending with 6  : {string.Join(", ", db.SetScan(allUsersSet, "*6"))}");

// Often times, you might want to atomically move an element between sets. To do that, all you need to do is call SetMove:
Console.WriteLine("Moving User:1 from active to offline");
var moved = db.SetMove(activeUsersSet, offlineUsersSet, "User:1");
Console.WriteLine($"Move Successful: {moved}");
// end coding challenge