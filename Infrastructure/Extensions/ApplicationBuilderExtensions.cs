using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Globalization;

namespace Infrastructure.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static WebApplication UseInfrastructure(this WebApplication app, IConfiguration config)
		{

			app.UseStaticFiles();
			// 기본 File 저장소 등록
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
				RequestPath = new PathString("/Files")
			});
			// Middleware 사용
			app.UseMiddlewares(config);
			// Rout 사용
			app.UseRouting();
			// Cors 사용
			app.UseCors("CorsPolicy");
			// 인증 사용
			app.UseAuthentication();
			// 권한 사용
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				// 모든 서비스 권한 사용
				endpoints.MapControllers().RequireAuthorization();
			});
			// Swagger 문서 서비스 등록
			app.UseSwaggerDocumentation(config);
			return app;
		}
	}
}