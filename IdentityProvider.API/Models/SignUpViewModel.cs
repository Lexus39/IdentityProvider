using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.API.Models
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmedPassword { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
