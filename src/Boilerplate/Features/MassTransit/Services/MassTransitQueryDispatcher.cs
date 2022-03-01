using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

namespace Boilerplate.Features.MassTransit.Services
{
    public class MassTransitQueryDispatcher
        : IQueryDispatcher
    {
        private readonly IQueryDispatcher _decorated;
        private readonly IDistributedQueryDispatcher _dispatcher;

        public MassTransitQueryDispatcher(
            IQueryDispatcher decorated,
            IDistributedQueryDispatcher distributedQueryDispatcher)
        {
            _decorated = decorated;
            _dispatcher = distributedQueryDispatcher;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            if (_dispatcher.IsDistributed(query))
            {
                return await _dispatcher.DispatchAsync<M>(query);
            }

            return await _decorated.DispatchAsync<M>(query);
        }

        public Task<IModel> DispatchAsync(IQuery query)
        {
            return _decorated.DispatchAsync(query);
        }
    }
}
