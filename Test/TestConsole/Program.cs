using System;
using FiQi.Cache.Abstract;
using Microsoft.Extensions.DependencyInjection;
using FiQi.Cache.MemoryCache;
using FiQi.Cache.FiQiRedisCache;
using FiQi.Redis;

namespace TestConsole
{
    public class User
    {
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddFiQiMemoryCache();
            services.AddFiQiRedis((o) =>
            {
                o.Address = "192.168.3.2:6379";
                o.DatabaseName = "test";
                o.DefaultDatabase = 0;
                o.Environment = "test";
                o.Password = "123456";
            });
            services.AddFiQiRedisCache();
            IFiQiCache fiQiCache = services.BuildServiceProvider().GetService<IFiQiCache>();
            fiQiCache.SetAsync("tst", new User() { Name = "q" });
            //fiQiCache.Set("tst", "123",TimeSpan.FromSeconds(30));
            User a = fiQiCache.GetAsync<User>("tst").Result;
            Console.WriteLine("Hello World!");
        }
    }
}
