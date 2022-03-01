using Boilerplate.Features.Reactive.Events;

namespace Boilerplate.Features.Reactive.Services
{
    public interface IEventHub
    {
        void Connect(Action<IObservable<IEvent>> connect);

        void Connect(Func<IObservable<IEvent>, IDisposable> a);

        bool IsOpen { get; }

        void Open();

        void Dispatch(IEvent @event);

        void Close();
    }
}
