using Boilerplate.Features.Reactive.Events;
using System.Reactive.Subjects;

namespace Boilerplate.Features.Reactive.Services
{
    public class DefaultEventHub
        : IEventHub
    {
        private Subject<IEvent> _subject;

        public void Connect(Action<IObservable<IEvent>> connect)
        {
            connect.Invoke(_subject);
        }

        public void Connect(Func<IObservable<IEvent>, IDisposable> connect)
        {
            connect.Invoke(_subject);
        }

        public bool IsOpen { get; private set; }

        public void Open()
        {
            if (!IsOpen)
            {
                _subject = new Subject<IEvent>();
                IsOpen = true;
            }
        }

        public void Dispatch(IEvent @event)
        {
            if (IsOpen)
            {
                _subject.OnNext(@event);
            }
        }

        public void Close()
        {
            IsOpen = false;
        }
    }
}
