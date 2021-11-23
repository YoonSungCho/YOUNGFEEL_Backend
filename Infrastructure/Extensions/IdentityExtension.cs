using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Persistence;
using Application.Exceptions;
using Application.Setting;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Extensions;

namespace Infrastructure.Extensions
{
	public static class IdentityExtension
	{
		internal static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
		{
			services
				.Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)))
				.AddIdentity<BaseUser, BaseRole>(options =>
				{
					options.Password.RequiredLength = 6;
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
					options.User.RequireUniqueEmail = true;
					options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._  @+";
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			services.AddJwtAuthentication(config);

			services.AddScoped<IBaseRoleStore<BaseRole>, BaseRoleStore>();
			services.AddScoped<BaseRoleManager>();
			services.AddScoped<IUserStore<BaseUser>, BaseUserStore>();
			return services;
		}

		internal static IServiceCollection AddJwtAuthentication(
			this IServiceCollection services, IConfiguration config)
		{
			var jwtSettings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
			byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);
			services
				.AddAuthentication(authentication =>
				{
					authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(bearer =>
				{
					bearer.RequireHttpsMetadata = false;
					bearer.SaveToken = true;
					bearer.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = false,
						ValidateLifetime = true,
						ValidateAudience = false,
						RoleClaimType = ClaimTypes.Role,
						ClockSkew = TimeSpan.Zero
					};
					bearer.Events = new JwtBearerEvents
					{
						OnChallenge = context =>
						{
							context.HandleResponse();
							if (!context.Response.HasStarted)
							{
								throw new IdentityException("Authentication Failed.", statusCode: HttpStatusCode.Unauthorized);
							}

							return Task.CompletedTask;
						},
						OnForbidden = context =>
						{
							throw new IdentityException("You are not authorized to access this resource.", statusCode: HttpStatusCode.Forbidden);
						},
						OnAuthenticationFailed = c =>
						{
							throw new IdentityException("Authentication Failed.", statusCode: HttpStatusCode.Unauthorized);
						}
					};
				});
			return services;
		}
	}
}