using Boilerplate.Features.Core.Queries;
using MassTransit;

namespace Boilerplate.Features.MassTransit.Services
{
    public class QueryConsumer<T>
        : IConsumer<T> where T : class, IQuery
    {
        public IQueryDispatcher _dispatcher;

        public QueryConsumer(IQueryDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            var model = await _dispatcher.DispatchAsync(context.Message);

            await context.RespondAsync(model, model.GetType());
        }
    }
}
