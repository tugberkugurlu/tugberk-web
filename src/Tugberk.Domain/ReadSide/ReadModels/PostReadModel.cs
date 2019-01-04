using System.Collections.Generic;
using System.Linq;

namespace Tugberk.Domain.ReadSide.ReadModels
{
    public class PostReadModel
    {
        public string Id { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Abstract  { get; set; }
        public string Content { get; set; }

        public ChangeRecordReadModel CreationRecord { get; set; }
        public IReadOnlyCollection<ChangeRecordReadModel> UpdateRecords { get; set; }

        public IReadOnlyCollection<UserReadModel> Authors { get; set; }
        public IReadOnlyCollection<SlugReadModel> Slugs { get; set; }
        public IReadOnlyCollection<TagReadModel> Tags { get; set; }
        public IReadOnlyCollection<CommentStatusActionRecordReadModel> CommentStatusActions { get; set; }
        public ApprovalStatusReadModel ApprovalStatus { get; set; }

        public SlugReadModel DefaultSlug => Slugs.Where(x => x.IsDefault)
            .OrderByDescending(x => x.CreatedOn)
            .First();

        public bool IsApproved => ApprovalStatus == ApprovalStatusReadModel.Approved;
    }
}
