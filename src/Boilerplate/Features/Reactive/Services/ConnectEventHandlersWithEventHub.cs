using Autofac;
using Boilerplate.Features.Reactive.Events;

namespace Boilerplate.Features.Reactive.Services
{
    public class ConnectEventHandlersWithEventHub
        : IEventHub
    {
        private readonly IEventHub _decorated;
        private readonly IEventHandlerRegistry _registry;
        private readonly List<IEventHandler> _handlers;
        private readonly ILifetimeScope _scope;

        public ConnectEventHandlersWithEventHub(
            IEventHub decorated,
            IEventHandlerRegistry registry,
            ILifetimeScope scope)
        {
            _decorated = decorated;
            _registry = registry;
            _handlers = new List<IEventHandler>();
            _scope = scope;
        }

        public bool IsOpen => _decorated.IsOpen;

        public void Connect(Action<IObservable<IEvent>> a)
        {
            _decorated.Connect(a);
        }

        public void Connect(Func<IObservable<IEvent>, IDisposable> a)
        {
            _decorated.Connect(a);
        }

        public void Open()
        {
            _decorated.Open();

            foreach (var handler in GetHandlers())
            {
                _decorated.Connect(stream => handler.Connect(stream));
            }
        }

        public void Dispatch(IEvent @event)
        {
            _decorated.Dispatch(@event);
        }

        public void Close()
        {
            _decorated.Close();

            foreach (var handler in _handlers)
            {
                handler.Dispose();
            }

            _handlers.Clear();
        }

        private List<IEventHandler> GetHandlers()
        {
            _handlers.Clear();

            foreach (var handlerType in _registry.GetHandlers())
            {
                IEventHandler handler = CreateHandler(handlerType);

                if (handler != null)
                {
                    _handlers.Add(handler);
                }
            }

            return _handlers;
        }

        private IEventHandler CreateHandler(Type handlerType)
        {
            return (IEventHandler)_scope.Resolve(handlerType);
        }
    }
}
