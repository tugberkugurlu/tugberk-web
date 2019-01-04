using System;

namespace Tugberk.Domain.ReadSide
{
    public class FoundByNonDefaultSlugResult<TModel> : FoundResult<TModel> where TModel : class
    {
        public FoundByNonDefaultSlugResult(TModel model, Slug defaultSlug) : base(model)
        {
            DefaultSlug = defaultSlug ?? throw new ArgumentNullException(nameof(defaultSlug));
        }

        public Slug DefaultSlug { get; }
    }
}
