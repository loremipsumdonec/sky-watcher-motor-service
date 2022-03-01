namespace Boilerplate.Features.Reactive.Events
{
    public class EventStack
        : IEventStack
    {
        private readonly List<IEvent> _register;

        public EventStack()
        {
            _register = new List<IEvent>();
        }

        public bool Suppress(IEvent @event)
        {
            if (@event is ISuppressEvent suppressEvent)
            {
                IEvent exists = _register.Find(s => s is ISuppressEvent eventInStack && eventInStack.Key == suppressEvent.Key);
                return exists != null;
            }

            return false;
        }

        public void Push(IEvent @event)
        {
            _register.Add(@event);
        }

        public void Pop()
        {
            if (_register.Count > 0)
            {
                _register.Remove(_register.Last());
            }
        }
    }
}