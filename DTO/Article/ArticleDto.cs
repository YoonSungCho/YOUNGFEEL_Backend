using System.ComponentModel.DataAnnotations;

namespace DTO.Workspace
{
	public class ArticleDto : IBaseDto
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? Status { get; set; }
        public Guid Id { get; set; }
    }
}