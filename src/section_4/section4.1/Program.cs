// See https://aka.ms/new-console-template for more information

using StackExchange.Redis;

var muxer = ConnectionMultiplexer.Connect("redis-12456.c251.east-us-mz.azure.cloud.redislabs.com:12456,password=Qfg0Zns0iXn4Kbq6RjKhNde7Rf0KzWHq");
var db = muxer.GetDatabase();


await db.KeyDeleteAsync("bf");
await db.KeyDeleteAsync("cms");
await db.KeyDeleteAsync("topk");

// Likely incomplete list of delimiting characters within the book.
char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n', '—', '?', '"', ';', '!', '’', '\r', '\'', '(', ')', '”' };

// Pull in text of Moby Dick.
var text = await File.ReadAllTextAsync("data/moby_dick.txt");

// Split words out from text.
var words = text.Split(delimiterChars).Where(s=>!string.IsNullOrWhiteSpace(s)).Select(s=>s.ToLower()).Select(x=>x).ToArray();

// Organize our words into a list to be pushed into the bloom filter in one shot, we could de-duplicate to make the transit shorter,
// but there's nothing inherently wrong with sending duplicates as the filter will filter them out.
var bloomList = words.Aggregate(new HashSet<object> { "bf" }, (list, word) =>
{
    list.Add(word);
    return list;
});

// Reserve our bloom filter.
await db.ExecuteAsync("BF.RESERVE", "bf", 0.01, 20000);

// Add All the Words to our bloom filter.
await db.ExecuteAsync("BF.MADD", bloomList, CommandFlags.FireAndForget);

// Reserve the Top-K.
await db.ExecuteAsync("TOPK.RESERVE", "topk", 10, 20, 10, .925);

// We need to organize the words into a list where each word is followed by the number of occurrences it has in Moby Dick.
var topKList = words.Aggregate(new Dictionary<string, int>(), (dict, word) =>
{
    if (!dict.ContainsKey(word))
    {
        dict.Add(word, 0);
    }

    dict[word]++;
    return dict;
}).Aggregate(new List<object> {"topk"}, (list, kvp) =>
{
   list.Add(kvp.Key);
   list.Add(kvp.Value);
   return list;
});

// Add everything to the Top-K.
await db.ExecuteAsync("TOPK.INCRBY", topKList, CommandFlags.FireAndForget);

// Ask the Bloom Filter and Top-K some questions...
var doesTheExist = await db.ExecuteAsync("BF.EXISTS", "bf", "the");

var doesTheExistAsInt = (int)doesTheExist;
Console.WriteLine($"Typeof {nameof(doesTheExistAsInt)}: {doesTheExistAsInt.GetType()}");

var doesTheExistAsDouble = (double)doesTheExist;
Console.WriteLine($"Typeof {nameof(doesTheExistAsDouble)}: {doesTheExistAsDouble.GetType()}");

Console.WriteLine($"Type enum for {nameof(doesTheExist)}: {doesTheExist.Type}");
Console.WriteLine($"Does 'the' exist in filter? {doesTheExist}'");

var res = await db.ExecuteAsync("TOPK.LIST", "topk");
var arr = ((RedisResult[])res!).Select(x=>x.ToString());
Console.WriteLine($"Top 10: {string.Join(", ", arr)}");

var withCounts = (await db.ExecuteAsync("TOPK.LIST", "topk", "WITHCOUNT")).ToDictionary().Select(x=>$"{x.Key}: {x.Value}");

Console.WriteLine($"Top 10, with counts: {string.Join(", ", withCounts)}");
