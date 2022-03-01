using Boilerplate.Features.Reactive.Events;

namespace Boilerplate.Features.Reactive.Services
{
    public class SuppressEventDispatcher
        : IEventDispatcher
    {
        private readonly IEventDispatcher _decorated;
        private readonly IEventStack _stack;

        public SuppressEventDispatcher(IEventDispatcher decorated, IEventStack stack)
        {
            _decorated = decorated;
            _stack = stack;
        }


        public void Dispatch(IEvent @event)
        {
            if (!_stack.Suppress(@event))
            {
                _stack.Push(@event);
                _decorated.Dispatch(@event);
                _stack.Pop();
            }
        }
    }
}
