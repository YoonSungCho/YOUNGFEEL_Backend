using System.Linq.Expressions;
using Application.Search;
using Application.Wrapper;
using Domain.Contracts;
using DTO;

namespace Application.Abstractions.Services.General
{
	public interface IGenericService : ITransientService
	{
		/// <summary>
		/// 	엔티티 조회
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<IResult<T>> GetByIdAsync<T>(Guid id) where T : IBaseDto;

		/// <summary>
		/// 	엔티티 리스트 조회
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<IResult<List<T>>> GetListAsync<T>() where T : IBaseDto;

		/// <summary>
		/// 	엔티티 리스트 조회 by userid
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<IResult<List<T>>> GetListByUserIdAsync<T>(Guid userId) where T : IBaseDto;

		/// <summary>
		/// 	엔티티 생성
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		Task<IResult<Guid>> CreateAsync<T>(T dto) where T : class;

		/// <summary>
		/// 	엔티티 존재 여부
		/// </summary>
		/// <returns></returns>
		Task<IResult<bool>> ExistsAsync<T>(T dto) where T : class;

		/// <summary>
		/// 	엔티티 수정
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		Task UpdateAsync<T>(T dto) where T : class;

		/// <summary>
		/// 	엔티티 삭제
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		Task RemoveAsync<T>(T dto) where T : class;

		/// <summary>
		/// 	엔티티 삭제 by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task RemoveByIdAsync(Guid id);
	}
}