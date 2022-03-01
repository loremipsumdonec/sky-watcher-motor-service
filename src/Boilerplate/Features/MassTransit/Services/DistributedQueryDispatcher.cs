using Autofac;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using MassTransit;
using System.Reflection;

namespace Boilerplate.Features.MassTransit.Services
{
    public class DistributedQueryDispatcher
        : IDistributedQueryDispatcher
    {   
        private readonly ILifetimeScope _scope;
        private readonly IQueryRegistry _registry;

        public DistributedQueryDispatcher(ILifetimeScope scope, IQueryRegistry registry)
        {
            _scope = scope;
            _registry = registry;
        }

        public bool IsDistributed(IQuery query)
        {
            return _registry.GetHandler(query) == null;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            return await DistributedDispatchAsync<M>(query);
        }

        private async Task<M> DistributedDispatchAsync<M>(IQuery query) where M : class, IModel
        {
            var genericRequestClientType = typeof(IRequestClient<>).MakeGenericType(query.GetType());

            var client = _scope.Resolve(genericRequestClientType);
            var method = GetResponseMethod(genericRequestClientType);

            var task = (Task)method
                .MakeGenericMethod(typeof(M))
                .Invoke(client, new object[] { query, default(CancellationToken), default(RequestTimeout) });

            await task.ConfigureAwait(false);
            var response = (Response)task.GetType().GetProperty("Result").GetValue(task);

            return (M)response.Message;
        }

        private static MethodInfo GetResponseMethod(Type genericRequestClientType)
        {
            var method = genericRequestClientType.GetMethods()
                .FirstOrDefault(m =>
                {
                    if (m.Name != "GetResponse")
                    {
                        return false;
                    }

                    var parameters = m.GetParameters();

                    if (parameters.Length > 2
                        && parameters[0].Name == "message"
                        && parameters[1].Name == "cancellationToken"
                        && m.GetGenericArguments().Length == 1)
                    {
                        return true;
                    }

                    return false;
                });

            if (method == null)
            {
                throw new InvalidOperationException("Could not find method GetResponse method on IRequestClient<> with reflection, check MassTransit for changes");
            }

            return method;
        }
    }
}
