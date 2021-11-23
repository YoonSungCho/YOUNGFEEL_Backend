using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Application.Abstractions.Services.Identity
{
	/// <summary>
	/// 	현재 로그인된 사용자 정보 인터페이스
	/// </summary>
	public interface ICurrentUser : ITransientService
	{
		string Name { get; }
		Guid GetUserId();
		string GetUserEmail();
		bool IsAuthenticated();
		bool IsInRole(string role);
		IEnumerable<Claim> GetUserClaims();
	}
}