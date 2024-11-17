using Fitness.Application.Identity.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Fitness.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
