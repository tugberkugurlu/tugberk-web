using Tugberk.Domain;

namespace Tugberk.Persistance.Abstractions
{
    public class PostFindResult
    {
        private PostFindResult(Post post)
        {
            IsSuccess = true;
            Post = post;
        }

        private PostFindResult(PostFindFailureReason failureReason)
        {
            IsSuccess = false;
            FailureReason = failureReason;
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
