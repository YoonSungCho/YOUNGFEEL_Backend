using Application.Wrapper;
using DTO.Identity.Requests;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.Identity
{
	/// <summary>
	/// 	IIdentityService 인터페이스
	/// </summary>
	public interface IIdentityService : ITransientService
	{
		/// <summary>
		/// 	유저 등록
		/// </summary>
		/// <param name="request"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		Task<IResult> RegisterAsync(RegisterRequest request, string origin);
		/// <summary>
		/// 	유저 로그인
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ipAddress">유저 IP (Jwt token 생성에 사용)</param>
		/// <returns></returns>
		Task<IResult> SignInAsync(TokenRequest request, string ipAddress);
	}
}