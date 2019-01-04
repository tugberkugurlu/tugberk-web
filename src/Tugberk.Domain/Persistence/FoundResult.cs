using System;

namespace Tugberk.Domain.Persistence
{
    public class FoundResult<TModel> : Result where TModel : class
    {
        public FoundResult(TModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public TModel Model { get; }
    }
}
