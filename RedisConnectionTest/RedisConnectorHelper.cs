using Firehose.Cache.Distributed.Redis;
using RedisShardingBundle;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RedisConnectionTest
{
    public class RedisConnectorHelper
    {
        private readonly CancellationTokenSource CancellationTokenSource;
        private readonly RedisConnectionsManager redisConnectionsManager;
        public RedisConnectorHelper(CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource;
            redisConnectionsManager = initRedisConnectionManager();
            
        }

        private RedisConnectionsManager initRedisConnectionManager() {
            IList<RedisConnectionConfig> connectionConfigs = new List<RedisConnectionConfig>
            {
                new RedisConnectionConfig("splicluster1.redis.cache.windows.net:6380,password=RrRDwXnjXqLeGTMkMNDfHyFLa1UPXSMjbOW7amRSnZQ=,connectTimeout=30000,ssl=True,abortConnect=False", 1),
                new RedisConnectionConfig("splitcluster2.redis.cache.windows.net:6380,password=DZEMUVwxoSdqL0xFhlot3jq+43v30QOERVnT7mXePao=,connectTimeout=30000,ssl=True,abortConnect=False", 1),
                new RedisConnectionConfig("splitcluster3.redis.cache.windows.net:6380,password=1Y32jYQzPtgz1dTNAZkCWIPkEDEUr6wdEpCQ9OfSzCs=,ssl=True,abortConnect=False", 1)
            };
            RedisConnectionsManager rcm = new RedisConnectionsManager(32, 3, 2, 1, connectionConfigs.ToArray(), CancellationTokenSource.Token);
            return rcm;
        }

        public bool StringSet(string key, string value)
        {
            return redisConnectionsManager.StringSet(key, key, value, TimeSpan.FromMinutes(2));
        }

        public string StringGet(string key)
        {
            RedisValue redisValue = redisConnectionsManager.StringGet(key, key);
            return redisValue.ToString();
        }
    }
}
