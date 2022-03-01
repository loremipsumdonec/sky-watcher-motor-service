using Boilerplate.Features.Reactive.Events;

namespace Boilerplate.Features.Reactive.Services
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent @event);
    }
}
