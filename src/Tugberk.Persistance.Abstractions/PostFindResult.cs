using System;
using Tugberk.Domain;

namespace Tugberk.Persistance.Abstractions
{
    public class PostFindResult
    {
        private PostFindResult(Post post)
        {
            Post = post ?? throw new ArgumentNullException(nameof(post));
            IsSuccess = true;
        }

        private PostFindResult(PostFindFailureReason failureReason)
        {
            FailureReason = failureReason;
            IsSuccess = false;
        }

        public bool IsSuccess { get; }
        public PostFindFailureReason FailureReason { get; }
        public Post Post { get; }

        public static PostFindResult Success(Post post) => 
            new PostFindResult(post);

        public static PostFindResult Fail(PostFindFailureReason failureReason) =>
            new PostFindResult(failureReason);
    }
}
