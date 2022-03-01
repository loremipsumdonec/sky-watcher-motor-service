using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using MassTransit;

namespace Boilerplate.Features.MassTransit.Services
{
    public class MassTransitEventHub
        : IEventHub
    {
        private readonly IEventHub _decorated;
        private readonly IPublishEndpoint _endpoint;

        public MassTransitEventHub(
            IEventHub decorated, 
            IPublishEndpoint endpoint
        )
        {
            _decorated = decorated;
            _endpoint = endpoint;
        }

        public bool IsOpen => _decorated.IsOpen;

        public void Close()
        {
            _decorated.Close();
        }

        public void Connect(Action<IObservable<IEvent>> connect)
        {
            _decorated.Connect(connect);
        }

        public void Connect(Func<IObservable<IEvent>, IDisposable> connect)
        {
            _decorated.Connect(connect);
        }

        public void Dispatch(IEvent @event)
        {
            if(IsNotAConsumedEvent(@event)) 
            {
                if(!IsOpen)
                {
                    return;
                }

                _endpoint.Publish(@event, @event.GetType());
            }

            _decorated.Dispatch(@event);
        }

        private bool IsNotAConsumedEvent(IEvent @event)
        {
            return true;
        }

        public void Open()
        {
            _decorated.Open();
        }
    }
}
