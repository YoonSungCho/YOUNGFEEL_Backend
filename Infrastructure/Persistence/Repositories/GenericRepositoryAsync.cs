using System.Linq.Expressions;
using Domain.Contracts;
using Application.Abstractions.Repositories;
using Application.Search;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Application.Exceptions;

namespace Infrastructure.Persistence.Repositories
{
	/// <summary>
	/// 	IGenericRepositoryAsync 구현 클래스
	/// 	모든 method 는 virtual 기술 되었기 때문에 customizing 가능
	/// </summary>
	public class GenericRepositoryAsync : IGenericRepositoryAsync
	{
		/// <summary>
		/// 	EF ORM
		/// </summary>
		private readonly ApplicationDbContext _dbContext;
		private readonly ILogger<GenericRepositoryAsync> _logger;

		public GenericRepositoryAsync(ApplicationDbContext dbContext, ILogger<GenericRepositoryAsync> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		/// <summary>
		///  	엔티티 생성
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task<Guid> CreateAsync<T>(T entity) where T : BaseEntity
		{
			await _dbContext.Set<T>().AddAsync(entity);
			return entity.Id;
		}

		/// <summary>
		/// 	엔티티 존재 여부 검사
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) where T : BaseEntity
		{
			IQueryable<T> query = _dbContext.Set<T>();
			if (expression != null) return await query.AnyAsync(expression, cancellationToken);
			return await query.AnyAsync(cancellationToken);
		}

		/// <summary>
		/// 	엔티티 조회
		/// </summary>
		/// <param name="id"></param>
		/// <param name="search"></param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task<T> GetByIdAsync<T>(Guid id, BaseSearch<T> search = null, CancellationToken cancellationToken = default) where T : BaseEntity
		{
			IQueryable<T> query = _dbContext.Set<T>();
			if (search != null)
				query = query.Specify(search);
			return await query.Where(e => e.Id == id).FirstOrDefaultAsync();
		}

		/// <summary>
		/// 	엔티티 리스트 조회
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="noTracking"></param>
		/// <param name="cancellationToken"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> expression, bool noTracking = false, CancellationToken cancellationToken = default) where T : BaseEntity
		{
			IQueryable<T> query = _dbContext.Set<T>();
			if (noTracking) query = query.AsNoTracking();
			if (expression != null) query = query.Where(expression);
			return await query.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// 	엔티티 삭제
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual Task RemoveAsync<T>(T entity) where T : BaseEntity
		{
			_dbContext.Set<T>().Remove(entity);
			return Task.CompletedTask;
		}

		/// <summary>
		/// 	엔티티 삭제 by id
		/// </summary>
		/// <param name="id"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task RemoveByIdAsync<T>(Guid id) where T : BaseEntity
		{
			var entity = await _dbContext.Set<T>().FindAsync(id);
			if (entity == null) throw new EntityNotFoundException("Entity not found.");
			_dbContext.Set<T>().Remove(entity);
		}

		/// <summary>
		/// 	Database 저장
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await _dbContext.SaveChangesAsync(cancellationToken);
		}

		/// <summary>
		/// 	엔티티 수정
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual async Task UpdateAsync<T>(T entity) where T : BaseEntity
		{
			if (_dbContext.Entry(entity).State == EntityState.Unchanged)
			{
				throw new NothingToUpdateException();
			}

			T exist = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id);
			if (exist == null) throw new EntityNotFoundException($"Entity not found : {entity.Id}");
			_dbContext.Entry(exist).CurrentValues.SetValues(entity);
		}
	}
}