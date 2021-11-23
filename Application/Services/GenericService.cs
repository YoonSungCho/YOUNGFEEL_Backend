using Application.Abstractions.Repositories;
using Application.Abstractions.Services.General;
using Application.Wrapper;
using Domain.Contracts;
using DTO;
using Microsoft.Extensions.Logging;
using Mapster;
using Application.Abstractions.Services.Identity;

namespace Application.Services
{
	/// <summary>
	/// 	서비스 공통 기능 추상 클래스
	/// </summary>
	public abstract class GenericService<TEntity> : IGenericService where TEntity : BaseEntity
	{

		private readonly ICurrentUser _currentUser;
		private readonly IGenericRepositoryAsync _genericRepositoryAsync;
		private readonly ILogger<GenericService<TEntity>> _logger;

		/// <summary>
		/// 	생성자
		/// </summary>
		/// <param name="genericRepositoryAsync"></param>
		public GenericService(ICurrentUser currentUser, IGenericRepositoryAsync genericRepositoryAsync, ILogger<GenericService<TEntity>> logger)
		{
			_currentUser = currentUser;
			_genericRepositoryAsync = genericRepositoryAsync;
			_logger = logger;
		}
		/// <summary>
		/// 	데이터 생성
		/// </summary>
		/// <param name="dto"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task<IResult<Guid>> CreateAsync<T>(T dto) where T : class
		{
			// T -> TEntity 변환
			var tEntity = dto.Adapt<TEntity>();
			// TEntity 생성
			var id = await _genericRepositoryAsync.CreateAsync<TEntity>(tEntity);
			// 저장
			await _genericRepositoryAsync.SaveChangesAsync();
			// 반환
			return await Result<Guid>.SuccessAsync(id);
		}

		public virtual async Task<IResult<bool>> ExistsAsync<T>(T dto) where T : class
		{
			throw new NotImplementedException();
		}

		public virtual async Task<IResult<T>> GetByIdAsync<T>(Guid id) where T : IBaseDto
		{
			var result = await _genericRepositoryAsync.GetByIdAsync<TEntity>(id);
			// DTO 로 변환 및 반환
			return await Result<T>.SuccessAsync(result.Adapt<T>());
		}

		public virtual async Task<IResult<List<T>>> GetListAsync<T>() where T : IBaseDto
		{
			// 삭제되지 않은 목록 조회
			// 전체 조회는 읽기 용으로 AsNoTrackring = true 사용
			var result = await _genericRepositoryAsync.GetListAsync<TEntity>(x => !x.IsDeleted, true);
			// DTO 로 변환 및 반환
			return await Result<List<T>>.SuccessAsync(result.Adapt<List<T>>());
		}

		public virtual async Task<IResult<List<T>>> GetListByUserIdAsync<T>(Guid userId) where T : IBaseDto
		{


			throw new NotImplementedException();
		}

		public virtual async Task RemoveAsync<T>(T dto) where T : class
		{
			throw new NotImplementedException();
		}

		public virtual async Task RemoveByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public virtual async Task UpdateAsync<T>(T dto) where T : class
		{
			throw new NotImplementedException();
		}
	}
}