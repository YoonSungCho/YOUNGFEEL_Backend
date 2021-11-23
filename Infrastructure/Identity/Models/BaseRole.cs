using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models
{
	public class BaseRole : IdentityRole<Guid>
	{
		public string Description { get; set; }

		public BaseRole()
		: base()
		{
		}

		public BaseRole(string roleName, string description = null)
		: base(roleName)
		{
			Description = description;
			NormalizedName = roleName.ToUpper();
		}
	}
}