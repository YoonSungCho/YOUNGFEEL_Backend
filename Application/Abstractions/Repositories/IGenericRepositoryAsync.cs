using System.Linq.Expressions;
using Domain.Contracts;
using Application.Abstractions.Services;
using Application.Search;

namespace Application.Abstractions.Repositories
{
	/// <summary>
	/// 	IGenericRepositoryAsync 인터페이스
	/// </summary>
	public interface IGenericRepositoryAsync : ITransientService
	{
		/// <summary>
		/// 	엔티티 조회
		/// </summary>
		/// <param name="id"></param>
		/// <param name="search"></param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="T">BaseEntity</typeparam>
		/// <returns></returns>
		Task<T> GetByIdAsync<T>(Guid id, BaseSearch<T> search = null, CancellationToken cancellationToken = default)
	   	where T : BaseEntity;

		/// <summary>
		/// 	엔티티 리스트 조회
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="noTracking"></param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> expression, bool noTracking = false, CancellationToken cancellationToken = default)
		where T : BaseEntity;

		/// <summary>
		/// 	엔티티 생성
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task<Guid> CreateAsync<T>(T entity)
		where T : BaseEntity;

		/// <summary>
		/// 	엔티티 존재 여부
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
		where T : BaseEntity;

		/// <summary>
		/// 	엔티티 수정
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task UpdateAsync<T>(T entity)
		where T : BaseEntity;

		/// <summary>
		/// 	엔티티 삭제
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task RemoveAsync<T>(T entity)
		where T : BaseEntity;

		/// <summary>
		/// 	엔티티 삭제 by id
		/// </summary>
		/// <param name="id"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task RemoveByIdAsync<T>(Guid id)
		where T : BaseEntity;

		/// <summary>
		/// 	Database 저장
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


	}
}