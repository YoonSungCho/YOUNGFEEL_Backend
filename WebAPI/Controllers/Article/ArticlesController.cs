using Microsoft.AspNetCore.Mvc;
using Application.Abstractions.Services.Article;
using DTO.Workspace;
using Swashbuckle.AspNetCore.Annotations;
using DTO.Workspace.request;
using Application.Abstractions.Services.General;

namespace WebAPI.Controllers.Workspace
{
	[ApiController]
	[Route("api/[controller]")]
	public class ArticlesController : ControllerBase
	{
		private readonly IArticleService _workspaceService;

		public ArticlesController(IArticleService workspaceService)
		{
			_workspaceService = workspaceService;
		}


		[HttpPost]
		[SwaggerOperation(Summary = "Create an article")]
		public async Task<IActionResult> CreateAsync(ArticleCreateRequest request)
		{
			return Ok(await _workspaceService.CreateAsync(request));
		}

		[HttpGet]
		[SwaggerOperation(Summary = "Get all articles")]
		public async Task<IActionResult> GetListAsync()
		{
			return Ok(await _workspaceService.GetListAsync<ArticleDto>());
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Summary = "Get an article by id")]
		public async Task<IActionResult> GetByIdAsync(Guid id)
		{
			return Ok(await _workspaceService.GetByIdAsync<ArticleDto>(id));
		}
	}
}