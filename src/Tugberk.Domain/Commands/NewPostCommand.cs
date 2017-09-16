using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tugberk.Domain.Commands 
 {
     public class NewPostCommand 
     {
         public NewPostCommand(string title, string @abstract, string content, PostFormat format, string ipAddress, User createdBy, IReadOnlyCollection<string> tags)
         {
            Title = title ?? throw new System.ArgumentNullException(nameof(title));
            Abstract = @abstract ?? throw new System.ArgumentNullException(nameof(@abstract));
            Content = content ?? throw new System.ArgumentNullException(nameof(content));
            IPAddress = ipAddress ?? throw new System.ArgumentNullException(nameof(ipAddress));
            CreatedBy = createdBy ?? throw new System.ArgumentNullException(nameof(createdBy));
            Tags = tags ?? throw new System.ArgumentNullException(nameof(tags));
            Format = format;
        }

         public string Title { get; }
         public string Abstract { get; }
         public string Content { get; }
         public PostFormat Format { get; }
         public string IPAddress { get; }
         public User CreatedBy { get; }

         public IReadOnlyCollection<string> Tags { get; }
     }
 }