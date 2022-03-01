namespace Boilerplate.Features.Reactive.Events
{
    public interface IEventStack
    {
        bool Suppress(IEvent @event);

        void Push(IEvent @event);

        void Pop();
    }
}