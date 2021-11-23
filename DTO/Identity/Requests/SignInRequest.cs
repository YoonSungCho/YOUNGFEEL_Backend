using System.ComponentModel.DataAnnotations;

namespace DTO.Identity.Requests
{
	public class SignInRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}