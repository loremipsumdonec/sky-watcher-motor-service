namespace Boilerplate.Features.Reactive.Events
{
    public interface IEventHandler
        : IDisposable
    {
        void Connect(IObservable<IEvent> stream);
    }

    public abstract class EventHandler
        : IEventHandler
    {
        private bool _disposed;

        ~EventHandler()
        {
            Dispose(false);
        }

        protected List<IDisposable> Disposables { get; } = new List<IDisposable>();

        public abstract void Connect(IObservable<IEvent> stream);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Disposables.ForEach(d => d.Dispose());
                Disposables.Clear();
            }

            _disposed = true;
        }
    }
}