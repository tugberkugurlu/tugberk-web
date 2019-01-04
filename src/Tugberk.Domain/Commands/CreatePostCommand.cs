using System.Collections.Generic;

namespace Tugberk.Domain.Commands 
 {
     public class CreatePostCommand 
     {
         public CreatePostCommand(string title, string @abstract, string content, string ipAddress, User createdBy, IReadOnlyCollection<string> tags, bool approved)
         {
             Title = title ?? throw new System.ArgumentNullException(nameof(title));
             Abstract = @abstract ?? throw new System.ArgumentNullException(nameof(@abstract));
             Content = content ?? throw new System.ArgumentNullException(nameof(content));
             IPAddress = ipAddress ?? throw new System.ArgumentNullException(nameof(ipAddress));
             CreatedBy = createdBy ?? throw new System.ArgumentNullException(nameof(createdBy));
             Tags = tags ?? throw new System.ArgumentNullException(nameof(tags));
             Approved = approved;
             Slug = Title.ToSlug();
         }

         public string Title { get; }
         public string Abstract { get; }
         public string Content { get; }
         public string IPAddress { get; }
         public User CreatedBy { get; }
         public bool Approved { get; }
         public string Slug { get; }
         public IReadOnlyCollection<string> Tags { get; }

         public class User
         {
             public string Id { get; set; }
             public string Name { get; set; }
         }
    }
 }