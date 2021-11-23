using Application.Abstractions.Repositories;
using Application.Abstractions.Services.Identity;
using Application.Abstractions.Services.Article;
using Application.Wrapper;
using DTO;
using DTO.Workspace;
using DTO.Workspace.request;
using Microsoft.Extensions.Logging;
using ArticleClass = Domain.Entities.Article.Article;

namespace Application.Services.Workspace
{
	/// <summary>
	/// 	워크스페이스 서비스 클래스
	/// </summary>
	public class ArticleService : GenericService<ArticleClass>, IArticleService
	{
		private readonly ICurrentUser _currentUser;
		private readonly IGenericRepositoryAsync _repository;
		private readonly ILogger<ArticleService> _logger;

		public ArticleService(ICurrentUser currentUser, IGenericRepositoryAsync repository, ILogger<ArticleService> logger) : base(currentUser, repository, logger)
		{
			_currentUser = currentUser;
			_repository = repository;
			_logger = logger;
		}

		/// <summary>
		/// 	Article 생성
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public override async Task<IResult<Guid>> CreateAsync<WorkspaceCreateRequest>(WorkspaceCreateRequest request)
		{
			return await base.CreateAsync(request);
		}

		public override async Task<IResult<List<WorkspaceDto>>> GetListAsync<WorkspaceDto>()
		{
			return await base.GetListAsync<WorkspaceDto>();
		}
	}
}