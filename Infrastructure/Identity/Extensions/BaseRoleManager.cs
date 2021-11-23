using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity.Extensions
{
	/// <summary>
	/// 	RoleManager 확장 Manager
	/// 	기본 RoleManager 기능 + custom methods 를 활용하기 위해 확장함
	/// </summary>
	public class BaseRoleManager : RoleManager<BaseRole>, IDisposable
	{
		private readonly IBaseRoleStore<BaseRole> _store;
		private readonly CancellationToken _cancel;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="store"></param>
		/// <param name="roleValidators"></param>
		/// <param name="keyNormalizer"></param>
		/// <param name="errors"></param>
		/// <param name="logger"></param>
		/// <param name="contextAccessor"></param>
		public BaseRoleManager(IBaseRoleStore<BaseRole> store,
			IEnumerable<IRoleValidator<BaseRole>> roleValidators,
			ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors,
			ILogger<RoleManager<BaseRole>> logger,
			IHttpContextAccessor contextAccessor)
			: base(store, roleValidators, keyNormalizer, errors, logger)
		{
			_store = store;
			_cancel = contextAccessor?.HttpContext?.RequestAborted ?? CancellationToken.None;
		}

		/// <summary>
		/// 	The cancellation token associated with the current HttpContext.RequestAborted or CancellationToken.None if unavailable.
		/// </summary>
		protected override CancellationToken CancellationToken => _cancel;

		/// <summary>
		/// 	특정 유저의 롤을 반환하는 함수
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<IEnumerable<BaseRole>> GetRolesByUserIdAsync(Guid userId)
		{
			return await _store.GetRolesByUserIdAsync(userId, CancellationToken);
		}
	}
}