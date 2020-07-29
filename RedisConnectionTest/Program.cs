using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace RedisConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            CancellationTokenSource cts = new CancellationTokenSource();
            RedisConnectorHelper rch = new RedisConnectorHelper(cts);

            Thread.Sleep(5000);

            while (true)
            {
                Console.WriteLine("Writing to redis");
                program.SaveBigData(rch);
                Console.WriteLine("Reading from cache");
                program.ReadData(rch);
                Console.WriteLine("To continue press y");
                var input = Console.ReadLine();
                if(!input.Equals("y"))
                {
                    break;
                }
            }
        }

        
        public void ReadData(RedisConnectorHelper cache)
        {
            var devicesCount = 20;
            for (int i = 1; i <= devicesCount; i++)
            {
                string key = $"Device_Status:{i}";
                var value = cache.StringGet(key);

                Console.WriteLine($"Key = {key}, Value = {value}");
            }
        }


        public void SaveBigData(RedisConnectorHelper cache)
        {
            var devicesCount = 20;
            var rnd = new Random();

            for (int i = 1; i <= devicesCount; i++)
            {
                string key = $"Device_Status:{i}";
                var value = rnd.Next(0, 100);
                cache.StringSet(key, value.ToString());
                Console.WriteLine($"Key = {key}, Value = {value}");
            }
        }
    }
}
