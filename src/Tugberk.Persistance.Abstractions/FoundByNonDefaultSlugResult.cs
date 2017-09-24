using System;
using Tugberk.Domain;

namespace Tugberk.Persistance.Abstractions
{
    public class FoundByNonDefaultSlugResult<TModel> : FoundResult<TModel> where TModel : class
    {
        public FoundByNonDefaultSlugResult(TModel model, Slug defaultSlug) : base(model)
        {
            if (defaultSlug == null) throw new ArgumentNullException(nameof(defaultSlug));

            DefaultSlug = defaultSlug;
        }

        public Slug DefaultSlug { get; }
    }
}
