using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public static class EntityExtensions 
    {
        // TODO: Add authors projection in its real form, not just out of createdby
        // TODO: Add CommentStatusActionRecord projection
        // TODO: Add ApprovalStatusActionRecord projection
        public static Post ToDomainModel(this PostEntity postEntity, IEnumerable<IdentityUserClaim<string>> authorClaims) 
        {
            var createdBy = postEntity.CreatedBy.ToDomainModel(authorClaims);

            return new Post 
            {
                Id = postEntity.Id.ToString(),
                Language = postEntity.Language,
                Title = postEntity.Title,
                Abstract = postEntity.Abstract,
                Content = postEntity.Content,
                Format = postEntity.Format.ToDomainModel(),
                Slugs = postEntity.Slugs.Select(x => x.ToDomainModel()).ToList().AsReadOnly(),
                Tags = postEntity.Tags.Select(x => x.ToDomainModel()).ToList().AsReadOnly(),
                Authors = new List<User> { createdBy }.AsReadOnly(),
                CreationRecord = new ChangeRecord 
                {
                    RecordedBy = createdBy,
                    RecordedOn = postEntity.CreatedOnUtc,
                    IpAddress = postEntity.CreationIpAddress
                }
            };
        }
        
        public static Tag ToDomainModel(this PostTagEntity tagEntity) =>
            new Tag 
            {
                Name = tagEntity.Tag.Name,
                Slugs = new List<Slug> { new Slug { Path = tagEntity.Tag.Slug, IsDefault = true } }
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