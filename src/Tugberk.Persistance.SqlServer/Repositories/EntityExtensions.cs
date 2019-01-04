using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Persistance.SqlServer.Repositories
{
    public static class EntityExtensions 
    {
        // TODO: Add authors projection in its real form, not just out of createdby
        // TODO: Add CommentStatusActionRecord projection
        // TODO: Add ApprovalStatusActionRecord projection
        public static PostReadModel ToReadModel(this PostEntity postEntity, IEnumerable<Claim> authorClaims) 
        {
            var createdBy = postEntity.CreatedBy.ToDomainModel(authorClaims);
            var slugs = postEntity.Slugs.Select(x => x.ToReadModel()).ToList().AsReadOnly();
            var tags = postEntity.Tags
                .Where(x => x.Tag != null) // it comes up null sometimes, no clue why as it's 'ThenInclude'd
                .Select(x => x.ToReadModel()).ToList().AsReadOnly();

            return new PostReadModel 
            {
                Id = postEntity.Id.ToString(),
                Language = postEntity.Language,
                Title = postEntity.Title,
                Abstract = postEntity.Abstract,
                Content = postEntity.Content,
                Slugs = slugs,
                Tags = tags,
                Authors = new List<UserReadModel> { createdBy }.AsReadOnly(),
                ApprovalStatus = postEntity.ApprovalStatus.ToDomainModel(),
                CreationRecord = new ChangeRecordReadModel 
                {
                    RecordedBy = createdBy,
                    RecordedOn = postEntity.CreatedOnUtc,
                    IpAddress = postEntity.CreationIpAddress
                }
            };
        }
        
        public static TagReadModel ToReadModel(this PostTagEntity tagEntity)
        {
            var tagName = tagEntity.Tag.Name;
            var tagSlug = tagEntity.Tag.Slug;

            return new TagReadModel
            {
                Name = tagName,
                Slugs = new List<SlugReadModel>
                {
                    new SlugReadModel { Path = tagSlug, IsDefault = true}
                }
            };
        }

        public static SlugReadModel ToReadModel(this PostSlugEntity slugEntity) =>
            new SlugReadModel
            {
                Path = slugEntity.Path,
                IsDefault = slugEntity.IsDefault,
                CreatedOn = slugEntity.CreatedOnUtc
            };
    }
}