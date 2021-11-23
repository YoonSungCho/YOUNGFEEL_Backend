using Infrastructure.Mapping;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			// Mapster 세팅
			MapsterSetting.Configure();
			// ITransientService, IScopedService 상속받는 서비스 등록
			services.AddServices(config);
			// 기본 setting 값 IOptions 등록
			services.AddSettings(config);
			// User 등록 밑 인증 절차 세팅 (Jwt Token)			
			services.AddIdentity(config);
			// Database 등록
			services.AddPersistence<ApplicationDbContext>(config);
			// Rout url 소문자 옵션 
			services.AddRouting(options => options.LowercaseUrls = true);
			// Middleware 등록 (exception 처리)
			services.AddMiddlewares(config);
			// Front-end 개발용 서버 Cors 설정
			services.AddCorsPolicy();
			// Swagger API 문서 등록
			services.AddSwaggerDocumentation(config);

			return services;
		}
	}
}