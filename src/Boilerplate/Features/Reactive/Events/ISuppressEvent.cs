namespace Boilerplate.Features.Reactive.Events
{
    public interface ISuppressEvent
        : IEvent
    {
        string Key { get; }
    }
}