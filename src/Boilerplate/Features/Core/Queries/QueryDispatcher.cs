using Autofac;
using Boilerplate.Features.Core.Commands;

namespace Boilerplate.Features.Core.Queries
{
    public class QueryDispatcher
        : IQueryDispatcher
    {
        private readonly ILifetimeScope _scope;
        private readonly IQueryRegistry _registry;

        public QueryDispatcher(IQueryRegistry registry, ILifetimeScope scope)
        {
            _registry = registry;
            _scope = scope;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            return (M)await DispatchAsync(query);
        }

        public Task<IModel> DispatchAsync(IQuery query)
        {
            IQueryHandler handler = GetHandler(query);

            if (handler != null)
            {
                return handler.ExecuteAsync(query);
            }
            else
            {
                return Task.FromResult<IModel>(null);
            }
        }

        private IQueryHandler GetHandler(IQuery query)
        {
            IQueryHandler handler = null;
            var handlerType = _registry.GetHandler(query);

            if (handlerType != null)
            {
                handler = (IQueryHandler)_scope.Resolve(handlerType);
                handler.Dispatcher = _scope.Resolve<IHeartbeatDispatcher>();
            }

            return handler;
        }
    }
}