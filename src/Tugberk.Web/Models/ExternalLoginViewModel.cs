using System.ComponentModel.DataAnnotations;

namespace Tugberk.Web.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}