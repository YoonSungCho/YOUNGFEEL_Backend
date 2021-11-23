using Infrastructure.Midllewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
	public static class MiddlewareExtensions
	{
		internal static IServiceCollection AddMiddlewares(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ExceptionMiddleware>();
			return services;
		}

		internal static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
		{
			app.UseMiddleware<ExceptionMiddleware>();
			return app;
		}
	}
}