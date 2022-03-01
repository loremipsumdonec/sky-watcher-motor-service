using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using MassTransit;

namespace Boilerplate.Features.MassTransit.Services
{
    public class EventConsumer<T>
        : IConsumer<T> where T : class, IEvent
    {
        public IEventDispatcher _dispatcher;

        public EventConsumer(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Task Consume(ConsumeContext<T> context)
        {
            _dispatcher.Dispatch(context.Message);
            return Task.CompletedTask;
        }
    }
}
