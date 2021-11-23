using Application.Setting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class SettingExtensions
    {
        internal static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
        {
            services
                .Configure<CorsSettings>(config.GetSection(nameof(CorsSettings)))
                .Configure<SwaggerSettings>(config.GetSection(nameof(SwaggerSettings)));
            return services;
        }
    }
}