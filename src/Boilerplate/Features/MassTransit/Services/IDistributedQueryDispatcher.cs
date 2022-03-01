using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace Boilerplate.Features.MassTransit.Services
{
    public interface IDistributedQueryDispatcher
    {
        public bool IsDistributed(IQuery query);

        public Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel;
    }
}
