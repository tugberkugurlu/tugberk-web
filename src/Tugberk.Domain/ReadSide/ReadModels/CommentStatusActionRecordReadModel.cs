using System;

namespace Tugberk.Domain.ReadSide.ReadModels
{
    public class CommentStatusActionRecordReadModel 
    {
        public CommentableStatusReadModel Status { get; set; }
        public UserReadModel RecordedBy { get; set; }
        public DateTime RecordedOn { get; set; }
    }
}
