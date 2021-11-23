using Application.Wrapper;
using DTO.Identity;
using DTO.Identity.Responses;

namespace Application.Abstractions.Services.Identity
{
	/// <summary>
	/// 	사용자 서비스 인터페이스
	/// </summary>
	public interface IUserService : ITransientService
	{
		/// <summary>
		/// 	모든 사용자 반환
		/// </summary>
		/// <returns></returns>
		Task<Result<List<UserDetailsDto>>> GetAllAsync();

		/// <summary>
		/// 	특정 사용자 반환
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<IResult<UserDetailsDto>> GetAsync(Guid userId);

		/// <summary>
		/// 	특정 사용자 수정
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<IResult<UserDetailsDto>> UpdateAsync(UserDetailsDto user);

		/// <summary>
		/// 	특정 사용자 삭제
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<IResult> RemoveAsync(Guid userId);
	}
}