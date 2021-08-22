using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace FiQi.Redis
{
    public static class AspNetExtension
    {
        public static IServiceCollection AddFiQiRedis(this IServiceCollection services, Action<FiQiRedisOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton<IFiQiRedisClient, FiQiRedisClient>();
            return services;
        }
    }
}
