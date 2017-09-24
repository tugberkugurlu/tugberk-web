using System;

namespace Tugberk.Persistance.Abstractions
{
    public class FoundResult<TModel> : Result where TModel : class
    {
        public FoundResult(TModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Model = model;
        }

        public TModel Model { get; }
    }
}
