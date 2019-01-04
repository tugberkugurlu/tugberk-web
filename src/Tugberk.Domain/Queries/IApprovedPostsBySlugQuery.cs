﻿using System.Threading.Tasks;
using OneOf;
using Optional;
using Tugberk.Domain.Persistence;

namespace Tugberk.Domain.Queries
{
    public interface IApprovedPostsBySlugQuery
    {
        Task<Option<OneOf<Post, NotApprovedResult<Post>>>> FindApprovedPostBySlug(string postSlug);
    }
}