using Fitness.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fitness.Shared
{
    public static class Extensions
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            services.AddScoped<IHttpContextService, HttpContextService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
