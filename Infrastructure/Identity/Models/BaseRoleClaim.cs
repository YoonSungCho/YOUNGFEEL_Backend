using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models
{
	public class BaseRoleClaim : IdentityRoleClaim<Guid>
	{
		public string Description { get; set; }
		public string TenantKey { get; set; }
		public string Group { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public int LastModifiedBy { get; set; }
		public DateTime? LastModifiedOn { get; set; }

		public BaseRoleClaim() : base()
		{
		}

		public BaseRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
		{
			Description = roleClaimDescription;
			Group = roleClaimGroup;
		}
	}
}