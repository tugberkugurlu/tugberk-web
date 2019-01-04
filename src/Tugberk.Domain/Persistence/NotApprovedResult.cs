namespace Tugberk.Domain.Persistence
{
    public class NotApprovedResult<TModel> : FoundResult<TModel> where TModel : class
    {
        public NotApprovedResult(TModel model) : base(model)
        {
        }
    }
}
