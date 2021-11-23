

using Domain.Contracts;

namespace Domain.Entities.Article
{
	public class Article : BaseEntity
	{
		public string Name { get; private set; }
		public string? Description { get; private set; }

		public Article() : base() { }
		public Article(string name) : base() { Name = name; }
		public Article(string name, string description) : base()
		{
			Name = name;
			Description = description;
		}
	}
}