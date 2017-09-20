using System.ComponentModel.DataAnnotations;

namespace Tugberk.Persistance.SqlServer
{
    public class PostSlugEntity : BaseSlugEntity
    {
        [Required]
        public PostEntity OwnedBy { get; set; }
    }
}
