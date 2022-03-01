using Boilerplate.Features.Reactive.Events;

namespace Boilerplate.Features.Reactive.Services
{
    public class DefaultEventDispatcher
        : IEventDispatcher
    {
        private readonly IEventHub _hub;

        public DefaultEventDispatcher(IEventHub hub)
        {
            _hub = hub;
        }

        public virtual void Dispatch(IEvent @event)
        {
            _hub.Dispatch(@event);
        }
    }
}
