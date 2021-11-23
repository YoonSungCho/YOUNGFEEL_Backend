using Infrastructure.Identity.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Extensions
{
	/// <summary>
	/// 	RoleStore 확장 클래스
	/// 	기본 RoleStore 클래스 상속 및 추가 메소드 제공
	/// </summary>
	public class BaseRoleStore : RoleStore<BaseRole, ApplicationDbContext, Guid>, IBaseRoleStore<BaseRole>
	{
		/// <summary>
		/// 	현재 scope 에서 사용되는 DbContext
		/// </summary>
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// 	생성자
		/// </summary>
		/// <param name="context"></param>
		/// <param name="describer"></param>
		/// <returns></returns>
		public BaseRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
		{
			_context = context;
		}

		/// <summary>
		/// 	현재 유저의 권한 값 반환
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="CancellationToken"></param>
		/// <returns></returns>
		public async Task<IEnumerable<BaseRole>> GetRolesByUserIdAsync(Guid userId, CancellationToken CancellationToken)
		{
			ThrowIfDisposed();
			var roleIds = await _context.UserRoles.Where(u => u.UserId.Equals(userId)).Select(u => u.RoleId).ToListAsync();
			if (roleIds.Any())
				return (await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync(CancellationToken)).OfType<BaseRole>();

			return new List<BaseRole>();
		}
	}
}