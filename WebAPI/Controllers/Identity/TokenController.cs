using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Services.Identity;
using DTO.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.Identity
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public sealed class TokenController : ControllerBase
	{
		private readonly ITokenService _tokenService;

		public TokenController(ITokenService tokenService)
		{
			_tokenService = tokenService;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> GetTokenAsync(TokenRequest request)
		{
			var token = await _tokenService.GetTokenAsync(request, GenerateIPAddress());
			return Ok(token);
		}

		[HttpPost("refresh")]
		[AllowAnonymous]
		public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
		{
			var response = await _tokenService.RefreshTokenAsync(request, GenerateIPAddress());
			return Ok(response);
		}

		private string GenerateIPAddress()
		{
			if (Request.Headers.ContainsKey("X-Forwarded-For"))
			{
				return Request.Headers["X-Forwarded-For"];
			}
			else
			{
				return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
			}
		}
	}
}