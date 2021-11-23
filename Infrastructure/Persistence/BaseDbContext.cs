using Application.Abstractions.Services.General;
using Application.Abstractions.Services.Identity;
using Application.Setting;
using Domain.Contracts;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence
{
	/// <summary>
	/// 	BaseDbContext 설정 클래스
	/// </summary>
	public abstract class BaseDbContext : IdentityDbContext<BaseUser, IdentityRole<Guid>, Guid>
	{
		private readonly ISerializerService _serializer;
		private readonly ICurrentUser _currentUserService;
		private readonly PersistSetting _persistSetting;


		/// <summary>
		/// 	생성자
		/// </summary>
		/// <param name="options"></param>
		/// <param name="persistSetting">DB 세팅 값</param>
		/// <param name="currentUserService"></param>
		/// <param name="serializer"></param>
		protected BaseDbContext(DbContextOptions options, IOptions<PersistSetting> persistSetting, ICurrentUser currentUserService, ISerializerService serializer)
		: base(options)
		{
			_currentUserService = currentUserService;
			_serializer = serializer;
			_persistSetting = persistSetting.Value;
		}

		/// <summary>
		/// 	Model 생성 이벤트 함수
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
			// Identity Table 정보 설정
			modelBuilder.ApplyIdentityConfiguration();
		}

		/// <summary>
		/// 	DB 연결 설정
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging();
			// TODO: DB 연결 설정이 누락됬을시 예외 처리 필요
			var connectionString = _persistSetting.ConnectionString;
			if (!string.IsNullOrEmpty(connectionString))
			{
				var dbProvider = _persistSetting.DBProvider;
				switch (dbProvider.ToLower())
				{
					case "postgresql":
						optionsBuilder.UseNpgsql(connectionString);
						break;
				}
			}
		}

		/// <summary>
		/// 	SaveChange 오버라이드 함수
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			var result = await base.SaveChangesAsync(cancellationToken);
			return result;
		}
	}
}