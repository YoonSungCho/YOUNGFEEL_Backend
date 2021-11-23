namespace Domain.Entities.Article
{
	public class UserArticle
	{
		public Guid UserId { get; set; }
		public Guid ArticleId { get; set; }
		public string Role { get; set; }

	}
}