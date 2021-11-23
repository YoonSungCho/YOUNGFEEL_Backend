using Application.Abstractions.Services.Identity;
using Application.Exceptions;
using Application.Setting;
using Application.Wrapper;
using DTO.Identity.Requests;
using DTO.Identity.Responses;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Infrastructure.Identity.Services
{
	/// <summary>
	/// 	토큰 생성 서비스
	/// </summary>
	public class TokenService : ITokenService
	{
		private readonly UserManager<BaseUser> _userManager;
		private readonly JwtSettings _config;

		/// <summary>
		/// 	토큰 생성 서비스 생성자
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="config"></param>
		public TokenService(
			UserManager<BaseUser> userManager,
			IOptions<JwtSettings> config)
		{
			_userManager = userManager;
			_config = config.Value;
		}

		/// <summary>
		/// 	토큰 생성
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public async Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
		{
			// 유저 조회
			var user = await _userManager.FindByEmailAsync(request.Email.Trim());
			if (user == null)
			{
				throw new IdentityException("identity.usernotfound", statusCode: HttpStatusCode.Unauthorized);
			}


			if (!user.IsActive)
			{
				throw new IdentityException("identity.usernotactive", statusCode: HttpStatusCode.Unauthorized);
			}

			// 패스워드 검증
			bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!passwordValid)
			{
				throw new IdentityException("identity.invalidcredentials", statusCode: HttpStatusCode.Unauthorized);
			}

			// 리프레쉬 토큰 생성 
			user.RefreshToken = GenerateRefreshToken();
			// 완료 날짜 (기본 7일)
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_config.RefreshTokenExpirationInDays);
			// 리프레쉬 토큰 저장
			await _userManager.UpdateAsync(user);
			// JWT 토큰 생성
			string token = GenerateJwt(user, ipAddress);
			// 결과 반환
			return await Result<TokenResponse>.SuccessAsync(new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime));
		}

		/// <summary>
		/// 	
		/// </summary>
		/// <param name="request"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
		{
			if (request is null)
			{
				throw new IdentityException("identity.invalidtoken", statusCode: HttpStatusCode.Unauthorized);
			}

			// 토큰 검사
			var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
			string userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(userEmail);
			if (user == null)
			{
				throw new IdentityException("identity.usernotfound", statusCode: HttpStatusCode.NotFound);
			}

			// 토큰이 다르거나 날짜가 지난 경우
			if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
			{
				throw new IdentityException("identity.invalidtoken", statusCode: HttpStatusCode.Unauthorized);
			}

			// 토큰 생성
			string token = GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
			// 리프레쉬 토큰 생성
			user.RefreshToken = GenerateRefreshToken();
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_config.RefreshTokenExpirationInDays);
			// 리프레쉬 토큰 업데이트
			await _userManager.UpdateAsync(user);
			var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
			return await Result<TokenResponse>.SuccessAsync(response);
		}

		/// <summary>
		/// 	JWT 토큰 생성
		/// </summary>
		/// <param name="user"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		private string GenerateJwt(BaseUser user, string ipAddress)
		{
			return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
		}

		private IEnumerable<Claim> GetClaims(BaseUser user, string ipAddress)
		{
			return new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(ClaimTypes.Email, user.Email),
				new("fullName", $"{user.FirstName} {user.LastName}"),
				new(ClaimTypes.Name, user.FirstName),
				new(ClaimTypes.Surname, user.LastName),
				new("ipAddress", ipAddress),
			};
		}
		/// <summary>
		/// 	리프레쉬 토큰 문자열 생성
		/// </summary>
		/// <returns></returns>
		private string GenerateRefreshToken()
		{
			byte[] randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		/// <summary>
		/// 	JWT 토큰 생성
		/// </summary>
		/// <param name="signingCredentials">보안 증명서</param>
		/// <param name="claims"></param>
		/// <returns></returns>
		private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
		{
			var token = new JwtSecurityToken(
			   claims: claims,
			   expires: DateTime.UtcNow.AddMinutes(_config.TokenExpirationInMinutes),
			   signingCredentials: signingCredentials);
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}

		private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key)),
				ValidateIssuer = false,
				ValidateAudience = false,
				RoleClaimType = ClaimTypes.Role,
				ClockSkew = TimeSpan.Zero,
				ValidateLifetime = false
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(
					SecurityAlgorithms.HmacSha256,
					StringComparison.InvariantCultureIgnoreCase))
			{
				throw new IdentityException("identity.invalidtoken", statusCode: HttpStatusCode.Unauthorized);
			}

			return principal;
		}

		/// <summary>
		/// 	보안 증명 발행
		/// </summary>
		/// <returns></returns>
		private SigningCredentials GetSigningCredentials()
		{
			byte[] secret = Encoding.UTF8.GetBytes(_config.Key);
			return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
		}
	}
}
