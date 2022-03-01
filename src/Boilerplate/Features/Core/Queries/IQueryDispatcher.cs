namespace Boilerplate.Features.Core.Queries
{
    public interface IQueryDispatcher
    {
        Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel;

        Task<IModel> DispatchAsync(IQuery query);
    }
}