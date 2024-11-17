using Fitness.Application.ApplicationServices;
using Fitness.Domain.Repositories;
using Fitness.Infrastructure.Reporitories;
using Fitness.Infrastructure.Repositories;
using Fitness.Infrastructure.Services;
using Fitness.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Fitness.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<ITariffRepository, TariffRepository>();
            services.AddScoped<ITimeIntervalRepository, TimeIntervalRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }
    }
}
