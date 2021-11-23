using Application.Setting;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


namespace Infrastructure.Persistence.Extensions
{
    public static class PersistenceExtensions
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(PersistenceExtensions));

        public static IServiceCollection AddPersistence<T>(this IServiceCollection services, IConfiguration config)
        where T : ApplicationDbContext
        {
            services.Configure<PersistSetting>(config.GetSection(nameof(PersistSetting)));
            var persistSetting = services.GetOptions<PersistSetting>(nameof(PersistSetting));
            var connectionString = persistSetting.ConnectionString;
            var dbProvider = persistSetting.DBProvider;
            if (string.IsNullOrEmpty(dbProvider)) throw new Exception("DB Provider is not configured.");
            _logger.Information($"Current DB Provider : {dbProvider}");
            switch (dbProvider.ToLower())
            {
                case "postgresql":
                    services.AddDbContext<T>(m => m.UseNpgsql(connectionString, e => e.MigrationsAssembly("Migrators.PostgreSQL")));
                    break;
                default:
                    throw new Exception($"DB Provider {dbProvider} is not supported.");
            }
            services.SetupDatabases<T>(persistSetting);
            return services;
        }

        private static IServiceCollection SetupDatabases<T>(this IServiceCollection services, PersistSetting options)
        where T : ApplicationDbContext
        {
            var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            dbContext.Database.SetConnectionString(options.ConnectionString);
            switch (options.DBProvider.ToLower())
            {
                case "postgresql":
                    services.AddDbContext<T>(m => m.UseNpgsql(e => e.MigrationsAssembly("Migrators.PostgreSQL")));
                    break;
                default:
                    throw new Exception($"DB Provider {options.DBProvider} is not supported.");
            }

            if (dbContext.Database.GetMigrations().Count() > 0)
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                    _logger.Information($"Applying Migrations.");
                }
            }

            return services;
        }


        public static T GetOptions<T>(this IServiceCollection services, string sectionName)
        where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);

            return options;
        }
    }
}