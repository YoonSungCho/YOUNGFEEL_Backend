using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Models
{
	public class BaseUser : IdentityUser<Guid>
	{
		[AdaptIgnore(MemberSide.Destination)]
		public new Guid Id { get; private set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsActive { get; set; }
		[AdaptIgnore]
		public string? RefreshToken { get; set; }
		[AdaptIgnore]
		public DateTime RefreshTokenExpiryTime { get; set; }
		public bool IsDeleted { get; set; }

		public BaseUser()
		{
			Id = Guid.NewGuid();
		}
	}
}
