using Application.Abstractions.Services.Identity;
using Application.Wrapper;
using Infrastructure.Identity.Models;
using DTO.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using DTO.Identity.Responses;
using Infrastructure.Identity.Extensions;

namespace Infrastructure.Identity.Services
{
	/// <summary>
	/// 	유저 서비스
	/// </summary>
	public class UserService : IUserService
	{
		private readonly UserManager<BaseUser> _userManager;
		private readonly BaseRoleManager _roleManager;

		public UserService(UserManager<BaseUser> userManager, BaseRoleManager roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager as BaseRoleManager;
		}

		/// <summary>
		/// 	모든 유저 조회
		/// </summary>
		/// <returns></returns>
		public async Task<Result<List<UserDetailsDto>>> GetAllAsync()
		{
			var users = await _userManager.Users.AsNoTracking().ToListAsync();
			var result = users.Adapt<List<UserDetailsDto>>();
			return await Result<List<UserDetailsDto>>.SuccessAsync(result);
		}

		/// <summary>
		/// 	유저 조회 by id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<IResult<UserDetailsDto>> GetAsync(Guid userId)
		{
			var user = await _userManager.Users.AsNoTracking().Where(u => u.Id == userId).FirstOrDefaultAsync();
			if (user == null) throw new EntityNotFoundException("User not found.");
			var result = user.Adapt<UserDetailsDto>();
			return await Result<UserDetailsDto>.SuccessAsync(result);
		}

		/// <summary>
		/// 	유저 삭제
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<IResult> RemoveAsync(Guid userId)
		{
			// 유저 조회
			var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
			if (user == null) throw new EntityNotFoundException("User not found.");
			// 삭제 처리
			user.IsDeleted = true;
			// 업데이트
			var result = await _userManager.UpdateAsync(user);
			// 실패 예외 처리
			if (!result.Succeeded) throw new CustomException("Update was fail.", result.Errors.Select(x => x.Description).ToList());

			return await Result.SuccessAsync();
		}

		/// <summary>
		/// 	유저 업데이트
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<IResult<UserDetailsDto>> UpdateAsync(UserDetailsDto request)
		{
			// 유저 조회
			var user = await _userManager.Users.Where(u => u.Id == request.Id).FirstOrDefaultAsync();
			if (user == null) throw new EntityNotFoundException("User not found.");
			// request 데이터 Mapping
			request.Adapt(user);
			// 업데이트
			var result = await _userManager.UpdateAsync(user);
			// 실패 예외 처리
			if (!result.Succeeded) throw new CustomException("Update was fail.", result.Errors.Select(x => x.Description).ToList());
			// 
			return await Result<UserDetailsDto>.SuccessAsync(user.Adapt<UserDetailsDto>());
		}
	}
}