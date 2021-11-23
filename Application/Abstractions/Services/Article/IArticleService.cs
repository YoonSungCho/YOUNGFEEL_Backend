using Application.Abstractions.Services;
using Application.Abstractions.Services.General;
using Application.Wrapper;
using DTO;
using DTO.Workspace;

namespace Application.Abstractions.Services.Article
{
	/// <summary>
	/// 	Article 인터페이스
	/// </summary>
	public interface IArticleService : IGenericService, ITransientService
	{

	}
}