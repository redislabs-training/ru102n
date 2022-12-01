using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace section3._3
{
    public class Redis
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyMuxer;

        static Redis()
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" },
                Password = ""
            };

            LazyMuxer = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }

        public static ConnectionMultiplexer Muxer => LazyMuxer.Value;
        public static IDatabase Database => Muxer.GetDatabase();
    }
}