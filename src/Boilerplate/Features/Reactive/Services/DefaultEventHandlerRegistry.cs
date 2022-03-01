namespace Boilerplate.Features.Reactive.Services
{
    public class DefaultEventHandlerRegistry
        : IEventHandlerRegistry
    {
        private class EventHandlerType
        {
            public EventHandlerType(Type type)
            {
                Type = type;
                Enabled = true;
            }

            public Type Type { get; set; }

            public bool Enabled { get; set; }
        }

        private readonly List<EventHandlerType> _registry;

        public DefaultEventHandlerRegistry()
        {
            _registry = new List<EventHandlerType>();
        }

        public void Add(Type type)
        {
            EventHandlerType exists = _registry.Find(e => e.Type == type);

            if (exists == null)
            {
                _registry.Add(new EventHandlerType(type));
            }
        }

        public void Enable(Type type)
        {
            EventHandlerType exists = _registry.Find(e => e.Type == type && !e.Enabled);

            if (exists != null)
            {
                exists.Enabled = true;
            }
        }

        public void Disable(Type type)
        {
            EventHandlerType exists = _registry.Find(e => e.Type == type && e.Enabled);

            if (exists != null)
            {
                exists.Enabled = false;
            }
        }

        public IEnumerable<Type> GetHandlers()
        {
            return _registry
                .Where(e => e.Enabled)
                .Select(e => e.Type);
        }

        public IEnumerable<Type> GetAllHandlers()
        {
            return _registry.Select(e => e.Type);
        }
    }
}
