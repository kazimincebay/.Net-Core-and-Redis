
using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();
await subscriber.SubscribeAsync("mychannel", (channel, message) =>
{
    Console.WriteLine(message);
});

//Console.Read();

/* pattern matching subscription - abonelerin belirli kalıplarda mesajları alabilmesi

await subscriber.SubscribeAsync("mychannel*", (channel, message) =>
{
    Console.WriteLine(message);
});

Console.Read();

Burada mychannel. ile başlayan mesajları kabul edecektir mychannel.google mychannel.facebook gibi kanalları dinler.
bu kanallara subscribe olmak için psubscribe mychannel.* şeklinde subscribe olunur.
*/