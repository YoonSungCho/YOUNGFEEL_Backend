using Mapster;

namespace Domain.Contracts
{
	/// <summary>
	/// 	BaseEntity 클래스
	/// </summary>
	public abstract class BaseEntity
	{
		/// <summary>
		/// 	고유 아이디
		/// </summary>
		/// <value></value>
		[AdaptIgnore(MemberSide.Destination)]
		public Guid Id { get; private set; }

		public Boolean IsDeleted { get; set; } = false;

		public BaseEntity()
		{
			Id = Guid.NewGuid();
		}
	}
}