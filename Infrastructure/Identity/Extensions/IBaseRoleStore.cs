using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Extensions
{
	/// <summary>
	/// 	IRoleStore 확장 인터페이스
	/// 	IRoleStore 기본 기능 + IBaseTRoleStre 확장 기능
	/// 	BaseRoleManager 에서 사용함
	/// </summary>
	/// <typeparam name="TRole"></typeparam>
	public interface IBaseRoleStore<TRole> : IRoleStore<TRole> where TRole : class
	{
		/// <summary>
		/// 	유저의 권한 값 반환
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="CancellationToken"></param>
		/// <returns></returns>
		Task<IEnumerable<TRole>> GetRolesByUserIdAsync(Guid userId, CancellationToken CancellationToken);
	}
}