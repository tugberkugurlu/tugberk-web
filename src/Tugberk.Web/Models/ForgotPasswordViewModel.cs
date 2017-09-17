using System.ComponentModel.DataAnnotations;

namespace Tugberk.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}