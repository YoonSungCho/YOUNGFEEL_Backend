using Infrastructure.Identity.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Identity.Extensions
{
	public class BaseUserStore : UserStore<BaseUser, BaseRole, ApplicationDbContext, Guid>
	{
		public BaseUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
	}
}