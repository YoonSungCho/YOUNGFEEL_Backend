using Application.Abstractions.Services.Identity;
using Application.Exceptions;
using Application.Wrapper;
using Infrastructure.Identity.Models;
using DTO.Identity.Requests;
using Microsoft.AspNetCore.Identity;
using Mapster;

namespace Infrastructure.Identity.Services
{
	/// <summary>
	/// 	유저 인증 서비스
	/// </summary>
	public class IdentityService : IIdentityService
	{
		private readonly SignInManager<BaseUser> _signInManager;
		private readonly UserManager<BaseUser> _userManager;
		private readonly ITokenService _tokenService;

		/// <summary>
		/// 	유저 인증 서비스 생성자
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="signInManager"></param>
		/// <param name="tokenService"></param>
		public IdentityService(
			UserManager<BaseUser> userManager,
			SignInManager<BaseUser> signInManager,
			ITokenService tokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
		}

		/// <summary>
		/// 	유저 등록
		/// </summary>
		/// <param name="request"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
		{
			// 유저 Email 검증
			var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email.Trim());
			if (userWithSameEmail != null) throw new IdentityException(string.Format("email {0} is already taken.", request.Email));

			var user = new BaseUser();
			user.IsActive = true;
			// request 데이터 Mapping
			request.Adapt(user);
			// userName
			user.UserName = $"{user.FirstName} {user.LastName}";
			// 유저 생성
			var result = await _userManager.CreateAsync(user, request.Password);
			// 예외 처리
			if (!result.Succeeded) throw new IdentityException("Validation Errors Occurred.", result.Errors.Select(a => a.Description).ToList());
			// 유저 결과 리턴
			var messages = new List<string> { string.Format("User {0} Registered.", user.UserName) };
			return await Result<Guid>.SuccessAsync(user.Id, messages: messages);
		}

		/// <summary>
		/// 	유저 로그인
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public async Task<IResult> SignInAsync(TokenRequest request, string ipAddress)
		{
			// 토큰 서비스 이용 
			// 유저 검증 완료되면 accessToken, refreshToken 반환
			return await _tokenService.GetTokenAsync(request, ipAddress);
		}
	}
}