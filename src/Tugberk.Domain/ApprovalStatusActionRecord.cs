using System;

namespace Tugberk.Domain
{
    public class ApprovalStatusActionRecord 
    {
        public ApprovalStatus Status { get; set; }
        public User RecordedBy { get; set; }
        public DateTime RecordedOn { get; set; }
    }
}
