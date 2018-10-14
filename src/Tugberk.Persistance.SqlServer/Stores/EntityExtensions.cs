using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public static class EntityExtensions 
    {
        // TODO: Add authors projection in its real form, not just out of createdby
        // TODO: Add CommentStatusActionRecord projection
        // TODO: Add ApprovalStatusActionRecord projection
        public static Post ToDomainModel(this PostEntity postEntity, IEnumerable<Claim> authorClaims) 
        {
            var createdBy = postEntity.CreatedBy.ToDomainModel(authorClaims);
            var slugs = postEntity.Slugs.Select(x => x.ToDomainModel()).ToList().AsReadOnly();
            var tags = postEntity.Tags
                .Where(x => x.Tag != null) // it comes up null sometimes, no clue why as it's 'ThenInclude'd
                .Select(x => x.ToDomainModel()).ToList().AsReadOnly();
            var approvalStatusActions = postEntity.ApprovalStatusActions.Select(x => new ApprovalStatusActionRecord 
                {
                    Status = x.Status.ToDomainModel(),
                    RecordedOn = x.RecordedOnUtc,
                    RecordedBy = new User 
                    {
                        Id = x.RecordedBy.Id
                    }
                }).ToList().AsReadOnly();

            return new Post 
            {
                Id = postEntity.Id.ToString(),
                Language = postEntity.Language,
                Title = postEntity.Title,
                Abstract = postEntity.Abstract,
                Content = postEntity.Content,
                Format = postEntity.Format.ToDomainModel(),
                Slugs = slugs,
                Tags = tags,
                Authors = new List<User> { createdBy }.AsReadOnly(),
                ApprovaleStatusActions = approvalStatusActions,
                CreationRecord = new ChangeRecord 
                {
                    RecordedBy = createdBy,
                    RecordedOn = postEntity.CreatedOnUtc,
                    IpAddress = postEntity.CreationIpAddress
                }
            };
        }
        
        public static Tag ToDomainModel(this PostTagEntity tagEntity)
        {
            var tagName = tagEntity.Tag.Name;
            var tagSlug = tagEntity.Tag.Slug;

            return new Tag
            {
                Name = tagName,
                Slugs = new List<Slug> {new Slug {Path = tagSlug, IsDefault = true}}
            };
        }

        public static Slug ToDomainModel(this PostSlugEntity slugEntity) =>
            new Slug 
            {
                Path = slugEntity.Path,
                IsDefault = slugEntity.IsDefault,
                CreatedOn = slugEntity.CreatedOnUtc
            };
    }
}