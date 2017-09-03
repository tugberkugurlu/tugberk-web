using System;

namespace Tugberk.Domain
{
    public class CommentStatusActionRecord 
    {
        public CommentableStatus Status { get; set; }
        public User RecordedBy { get; set; }
        public DateTime RecordedOn { get; set; }
    }
}
