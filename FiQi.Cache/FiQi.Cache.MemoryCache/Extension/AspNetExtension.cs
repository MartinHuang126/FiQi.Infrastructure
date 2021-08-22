using FiQi.Cache.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace FiQi.Cache.MemoryCache
{
    public static class AspNetExtension
    {
        public static IServiceCollection AddFiQiMemoryCache(this IServiceCollection services)
        {
            services.AddOptions<MemoryCacheOptions>();
            services.AddSingleton<IMemoryCache, Microsoft.Extensions.Caching.Memory.MemoryCache>();
            services.AddSingleton<IFiQiCache, FiQiMemoryCache>();
            return services;
        }
    }
}
