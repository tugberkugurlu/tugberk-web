using System;
using Tugberk.Domain;

namespace Tugberk.Web.Models
{
    public class HomePageViewModel
    {
        public HomePageViewModel(int currentPage, Paginated<Post> paginatedPostsResult)
        {
            if (currentPage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentPage));
            }

            CurrentPage = currentPage;
            PaginatedPostsResult = paginatedPostsResult ?? throw new ArgumentNullException(nameof(paginatedPostsResult));
        }

        public int CurrentPage { get; }
        public Paginated<Post> PaginatedPostsResult { get; }
    }
}