using Application.Abstractions.Services.General;
using Application.Abstractions.Services.Identity;
using Application.Setting;
using Domain.Contracts;
using Domain.Entities.Article;
using Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace Infrastructure.Persistence
{
	/// <summary>
	/// 	
	/// </summary>
	public class ApplicationDbContext : BaseDbContext
	{
		private readonly ISerializerService _serializer;
		public IDbConnection Connection => Database.GetDbConnection();
		private readonly ICurrentUser _currentUserService;
		public ApplicationDbContext(DbContextOptions options, IOptions<PersistSetting> persistSetting, ICurrentUser currentUserService, ISerializerService serializer)
		: base(options, persistSetting, currentUserService, serializer)
		{
			_currentUserService = currentUserService;
			_serializer = serializer;
		}

		public DbSet<Article> Articles { get; set; }
		public DbSet<UserArticle> UserArticles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<BaseUser>(b =>
			{
				b.HasMany<UserArticle>().WithOne().HasForeignKey(uw => uw.UserId).IsRequired();
			});

			modelBuilder.Entity<Article>(b =>
			{
				b.HasMany<UserArticle>().WithOne().HasForeignKey(uw => uw.ArticleId).IsRequired();
			});

			modelBuilder.Entity<UserArticle>(b =>
			{
				b.HasKey(r => new { r.UserId, r.ArticleId });
			});
		}

		/// <summary>
		/// 	SaveChange 오버라이드 함수
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}