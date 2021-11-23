using Application.Abstractions.Services.Identity;
using DTO.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.Identity
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		[SwaggerOperation(Summary = "Get all user list")]
		public async Task<IActionResult> GetAllAsync()
		{
			return Ok(await _userService.GetAllAsync());
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Summary = "Get a user by id")]
		public async Task<IActionResult> GetByIdAsync(Guid id)
		{
			return Ok(await _userService.GetAsync(id));
		}

		[HttpPut]
		[SwaggerOperation(Summary = "Update a user data")]
		public async Task<IActionResult> UpdateByAync(UserDetailsDto request)
		{
			return Ok(await _userService.UpdateAsync(request));
		}

		[HttpDelete("{id}")]
		[SwaggerOperation(Summary = "Remove a user by id")]
		public async Task<IActionResult> RemoveByAync(Guid id)
		{
			return Ok(await _userService.RemoveAsync(id));
		}
	}
}