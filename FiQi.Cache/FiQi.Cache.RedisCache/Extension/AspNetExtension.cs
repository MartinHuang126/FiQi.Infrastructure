using FiQi.Cache.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace FiQi.Cache.FiQiRedisCache
{
    public static class AspNetExtension
    {
        public static IServiceCollection AddFiQiRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<IFiQiCache, FiQiRedisCache>();
            return services;
        }
    }
}
