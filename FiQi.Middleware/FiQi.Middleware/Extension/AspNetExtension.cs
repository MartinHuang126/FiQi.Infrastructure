using Microsoft.AspNetCore.Builder;

namespace FiQi.Middleware
{
    public static class AspNetExtension
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorMiddleware>();
        }
    }
}
