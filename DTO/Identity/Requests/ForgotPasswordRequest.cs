using System.ComponentModel.DataAnnotations;

namespace DTO.Identity.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}