using System.Linq.Expressions;
using Domain.Contracts;

namespace Application.Search
{
	/// <summary>
	/// 	BaseEntity 조회시 search 조건 인터페이스
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISearch<T> where T : BaseEntity
	{
		Expression<Func<T, bool>> Criteria { get; }
		List<Expression<Func<T, object>>> Includes { get; }
		List<string> IncludeStrings { get; }
		Expression<Func<T, bool>> And(Expression<Func<T, bool>> query);
		Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query);
	}
}