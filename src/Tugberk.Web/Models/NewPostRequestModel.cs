using System.ComponentModel.DataAnnotations;

namespace Tugberk.Web.Models 
{
    public class NewPostRequestModel 
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }
    }
}