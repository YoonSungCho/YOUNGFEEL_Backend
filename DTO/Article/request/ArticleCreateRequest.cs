using System.ComponentModel.DataAnnotations;

namespace DTO.Workspace.request
{
	public class ArticleCreateRequest
	{
		[Required]
		public string Name { get; set; }
		public string? Description { get; set; }
	}
}