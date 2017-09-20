using System.Linq;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public static class EntityExtensions 
    {
        // TODO: Add authors projection
        // TODO: Add CommentStatusActionRecord projection
        // TODO: Add ApprovalStatusActionRecord projection
        public static Post ToDomainModel(this PostEntity postEntity) => 
            new Post 
            {
                Id = postEntity.Id.ToString(),
                Language = postEntity.Language,
                Title = postEntity.Title,
                Abstract = postEntity.Abstract,
                Content = postEntity.Content,
                Format = postEntity.Format.ToDomainModel(),
                Slugs = postEntity.Slugs.Select(x => x.ToDomainModel()).ToList().AsReadOnly(),
                CreationRecord = new ChangeRecord 
                {
                    RecordedBy = postEntity.CreatedBy.ToDomainModel(),
                    RecordedOn = postEntity.CreatedOnUtc,
                    IpAddress = postEntity.CreationIpAddress
                }
            };

        public static Slug ToDomainModel(this PostSlugEntity slugEntity) =>
            new Slug 
            {
                Path = slugEntity.Path,
                IsDefault = slugEntity.IsDefault,
                CreatedOn = slugEntity.CreatedOnUtc
            };
    }
}