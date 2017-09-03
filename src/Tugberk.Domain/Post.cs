using System.Collections.Generic;

namespace Tugberk.Domain
{
    public class Post
    {
        public string Id { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Abstract  { get; set; }
        public string Content { get; set; }
        public PostFormat Format { get; set; }

        public ChangeRecord CreationRecord { get; set; }
        public IReadOnlyCollection<ChangeRecord> UpdateRecords { get; set; }

        public IReadOnlyCollection<User> Authors { get; set; }
        public IReadOnlyCollection<CommentStatusActionRecord> CommentStatusActions { get; set; }
        public IReadOnlyCollection<ApprovalStatusActionRecord> ApprovaleStatusActions { get; set; }
    }
}
