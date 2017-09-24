using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tugberk.Persistance.SqlServer
{
    public class PostApprovalStatusActionEntity 
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public ApprovalStatusEntity Status { get; set; }

        [Required]
        public DateTime RecordedOnUtc { get; set; }

        [Required]
        public IdentityUser RecordedBy { get; set; }

        [Required]
        public PostEntity Post { get; set; }
    }
}
