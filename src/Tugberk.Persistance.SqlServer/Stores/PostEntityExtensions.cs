using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer.Stores
{
    public static class PostEntityExtensions 
    {
        // TODO: Add authors projection
        // TODO: Add slugs projection
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
                CreationRecord = new ChangeRecord 
                {
                    RecordedBy = postEntity.CreatedBy.ToDomainModel(),
                    RecordedOn = postEntity.CreatedOnUtc,
                    IpAddress = postEntity.CreationIpAddress
                }
            };
    }
}