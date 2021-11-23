using Application.Abstractions.Services.Identity;
using DTO.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Identity
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public sealed class IdentityController : ControllerBase
	{
		private readonly ICurrentUser _user;
		private readonly IIdentityService _identityService;
		private readonly IUserService _userService;
		private readonly ITokenService _tokenService;

		public IdentityController(IIdentityService identityService, ICurrentUser user, IUserService userService, ITokenService tokenService)
		{
			_identityService = identityService;
			_user = user;
			_userService = userService;
			_tokenService = tokenService;
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
		{
			string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{Request.PathBase.Value.ToString()}";
			string origin = string.IsNullOrEmpty(Request.Headers["origin"].ToString()) ? baseUrl : Request.Headers["origin"].ToString();
			return Ok(await _identityService.RegisterAsync(request, origin));
		}

		[HttpPost("signin")]
		[AllowAnonymous]
		public async Task<IActionResult> SignInAsync([FromForm] TokenRequest request)
		{
			return Ok(await _identityService.SignInAsync(request, GenerateIPAddress()));
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