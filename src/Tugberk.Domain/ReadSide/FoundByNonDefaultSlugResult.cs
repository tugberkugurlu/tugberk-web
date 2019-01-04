using System;
using Tugberk.Domain.ReadSide.ReadModels;

namespace Tugberk.Domain.ReadSide
{
    public class FoundByNonDefaultSlugResult<TModel> : FoundResult<TModel> where TModel : class
    {
        public FoundByNonDefaultSlugResult(TModel model, SlugReadModel defaultSlug) : base(model)
        {
            DefaultSlug = defaultSlug ?? throw new ArgumentNullException(nameof(defaultSlug));
        }

        public SlugReadModel DefaultSlug { get; }
    }
}
