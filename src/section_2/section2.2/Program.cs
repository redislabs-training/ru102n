using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("localhost");
var db = muxer.GetDatabase();
// TODO for Coding Challenge Start here on starting-point branch
// We'll maintain two lists, fruits and vegetables.
var fruitKey = "fruits";
var vegetableKey = "vegetables";
db.KeyDelete(new RedisKey[] { fruitKey, vegetableKey });
// Lists are doubly linked lists, meaning you can push or pop from 
// either side of the list. However, any sort of indexed access becomes
// an O(N) operation where N is the the number of elements you need to
// traverse to reach the index.

// Let's push some fruit in to the left side.
db.ListLeftPush(fruitKey, new RedisValue[]{"Banana", "Mango", "Apple", "Pepper", "Kiwi", "Grape"});

Console.WriteLine("=====Fruits=====");

// Now let's try printing out the first element in this list. Lists are indexed
// left to right, consequentially the last fruit that we pushed to the left
// will be at the 0th index - 'Grape':
Console.WriteLine($"The first fruit in the list is: {db.ListGetByIndex(fruitKey, 0)}");

// We can access the tail of the list by using the index -1:
Console.WriteLine($"The last fruit in the list is:  {db.ListGetByIndex(fruitKey, -1)}");

// Now, we can reverse the semantics of our insertions by using ListRightPush, which will
// push to the right side of the list (the tail):
db.ListRightPush(vegetableKey, new RedisValue[]{"Potato", "Carrot", "Asparagus", "Beet", "Garlic", "Tomato"});
Console.WriteLine("=====Vegetables=====");

// Now if we try to access the first element in the vegetables list, we'll get the first element
// that we pushed in from the right:
Console.WriteLine($"The first vegetable in the list is: {db.ListGetByIndex(vegetableKey, 0)}");

// Conversely the last element will be the last element we pushed:
Console.WriteLine($"The last vegetable in the list is:  {db.ListGetByIndex(vegetableKey, -1)}");

// We can get ranges of elements within a list by using the ListRange functionality. These ranges are left->right
// If you leave the start and stop index blank, you'll simply get the entire list:

Console.WriteLine($"Fruit indexes 0 to -1: {string.Join(", ", db.ListRange(fruitKey))}");

// You can also set a start and stop for the list range. If we go from 0 to -2, our vegetable list will exclude
// an element in the list, Tomato, which isn't technically a vegetable anyway.
Console.WriteLine($"Vegetables index 0 to -2: {string.Join(", ", db.ListRange(vegetableKey, 0, -2))}");

// In fact, Tomato is really not a vegetable, so we can move it from our vegetable list to our fruit list
// using the ListMove method. We'll move the Tomato from the tail of the vegetable list, and put it at the head of the
// Fruit list by taking from the right and placing on the left.
db.ListMove(vegetableKey, fruitKey, ListSide.Right, ListSide.Left);
Console.WriteLine($"The first fruit in the list is: {db.ListGetByIndex(fruitKey, 0)}");

// Redis Lists can be used as FIFO Queues by pushing left and popping right:
Console.WriteLine("Enqueuing Celery");
db.ListLeftPush(vegetableKey, "Celery");
Console.WriteLine($"Dequeued: {db.ListRightPop(vegetableKey)}");

// Redis Lists can also be used as a LIFO Stack by pushing and popping from a single side
// and you can pop more than one record at the same time by using the count argument.
Console.WriteLine("Pushing Grapefruit");
db.ListLeftPush(fruitKey, "Grapefruit");
Console.WriteLine($"Popping Fruit: {string.Join(",", db.ListLeftPop(fruitKey, 2))}");

// Redis Lists also allow you to find the index of a particular item:
Console.WriteLine($"Position of Mango: {db.ListPosition(fruitKey, "Mango")}");

// And finally, you use the Length method to determine the size of a given List.
Console.WriteLine($"There are {db.ListLength(fruitKey)} fruits in our Fruit List");
// end coding challenge