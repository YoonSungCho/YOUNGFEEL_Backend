using Application.Setting;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class CorsExtensions
    {
        internal static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            var corsSettings = services.GetOptions<CorsSettings>(nameof(CorsSettings));
            return services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(new string[] { corsSettings.React });
                });
            });
        }
    }
}