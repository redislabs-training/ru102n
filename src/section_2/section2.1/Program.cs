using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();
// TODO for Coding Challenge Start here on starting-point branch
// Strings are fundamentally the simplest of Redis Data Types
// They are a single RedisKey mapping to a single Redis Value
// In this exercise we'll go over how to use a variety of the 
// features offered by Redis Strings.


// Let's start off with the basics, lets map a single string
// to a single value. We'll create a new RedisKey and pass
// that, along with the value we'd like to store to
// our database. Note, there is an implicit conversion from
// strings to RedisKeys, so it's just as valid to have the 
// Key be a simple string.
var instructorNameKey = new RedisKey("instructors:1:name");

db.StringSet(instructorNameKey, "Steve");
 
// This stores the name Steve at the key instructors:1:name
// Now all we need to do to retrieve that string is to call
// StringGet, passing in our Key.

var instructor1Name = db.StringGet(instructorNameKey);

Console.WriteLine($"Instructor 1's name is: {instructor1Name}");

// Ok great, but now we just realized we were supposed to store
// the instructor's full name at that key, not just their first
// name. Well, we could completely overwrite the key, with another
// StringSet, but that would be inefficient, let's append instead!

db.StringAppend(instructorNameKey, " Lorello");
instructor1Name = db.StringGet(instructorNameKey);
Console.WriteLine($"Instructor 1's full name is: {instructor1Name}");

// Now let's look at some other encoding types for strings. The next
// obvious encoding type is numerics. For numerics, you can set
// the number in the Redis String as you would any other string.

var tempKey = "temperature";
db.StringSet(tempKey, 42);

// Now that we've set a number in a string, we can perform numeric
// operations on it. If we use StringIncrement, the IDatabase
// will choose the appropriate command for the the Data type
// For example, when passing in an integer, it will call the 
// INCRBY command under the hood, see below:
var tempAsLong = db.StringIncrement(tempKey, 5);
Console.WriteLine($"New temperature: {tempAsLong}");

// Of course you can just not pass in any increment, in which
// case it will simply increment it by 1:
tempAsLong = db.StringIncrement(tempKey);
Console.WriteLine($"New Temp: {tempAsLong}");

// You can also pass in a floating point number, which will 
// automatically call the INCRBYFLOAT method under the hood:
var tempAsDouble = db.StringIncrement(tempKey, .5);
Console.WriteLine($"New temperature: {tempAsDouble}");

// There are other options that you can pass in when you are
// running a StringSet. There's the expiry option, which is
// a timespan that will expire the key after the alloted time.
// For example, if we wanted a temporary key to last for one second
// we could use:
db.StringSet("temporaryKey", "hello world", expiry: TimeSpan.FromSeconds(1));

var conditionalKey = "ConditionalKey";
var conditionalKeyText = "this has been set";
// You can also specify a condition for when you want to set a key.
// For example, if you only want to set a key when it does not already
// exist, you can by specifying the NotExists condition:
var wasSet = db.StringSet(conditionalKey, conditionalKeyText, when: When.NotExists);
Console.WriteLine($"Key set: {wasSet}");

// Of course, after the key has been set, if you try to set the key again
// it will not work, and you will get false back from StringSet:
wasSet = db.StringSet(conditionalKey, "this text doesn't matter since it won't be set", when:When.NotExists);
Console.WriteLine($"Key set: {wasSet}");

// You can also use When.Exists, to set the key only if the key already exists:
wasSet = db.StringSet(conditionalKey, "we reset the key!");
Console.WriteLine($"Key set: {wasSet}");

db.KeyDelete(conditionalKey);
// end coding challenge