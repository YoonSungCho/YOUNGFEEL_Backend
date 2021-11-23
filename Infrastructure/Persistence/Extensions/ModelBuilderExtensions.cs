using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Extensions
{
	public static class ModelBuilderExtensions
	{

		public static void ApplyIdentityConfiguration(this ModelBuilder builder)
		{
			builder.Entity<BaseUser>(entity =>
			{
				entity.ToTable("Users", "Identity");
			});

			builder.Entity<BaseRole>(entity =>
			{
				entity.ToTable("Roles", "Identity");
			});

			builder.Entity<IdentityRole<Guid>>(entity =>
			{
				entity.ToTable("Roles", "Identity");
			});

			builder.Entity<BaseRoleClaim>(entity =>
			{
				entity.ToTable("RoleClaims", "Identity");
			});

			builder.Entity<IdentityRoleClaim<Guid>>(entity =>
			{
				entity.ToTable("RoleClaims", "Identity");
			});

			builder.Entity<IdentityUserRole<Guid>>(entity =>
			{
				entity.ToTable("UserRoles", "Identity");
			});

			builder.Entity<IdentityUserClaim<Guid>>(entity =>
			{
				entity.ToTable("UserClaims", "Identity");
			});

			builder.Entity<IdentityUserLogin<Guid>>(entity =>
			{
				entity.ToTable("UserLogins", "Identity");
			});
			builder.Entity<IdentityUserToken<Guid>>(entity =>
			{
				entity.ToTable("UserTokens", "Identity");
			});
		}

		public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
		{
			var entities = modelBuilder.Model
				.GetEntityTypes()
				.Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
				.Select(e => e.ClrType);
			foreach (var entity in entities)
			{
				var newParam = Expression.Parameter(entity);
				var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
				modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
			}
		}
	}
}