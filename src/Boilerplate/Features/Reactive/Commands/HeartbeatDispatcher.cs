using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;

namespace Boilerplate.Features.Reactive.Commands
{
    public class HeartbeatDispatcher
        : IHeartbeatDispatcher
    {
        private readonly IEventDispatcher _dispatcher;

        public HeartbeatDispatcher(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Dispatch(string message, Severitys severity, object data = null)
        {
            Heartbeat heartbeat = new Heartbeat(message)
            {
                Data = data,
                Severity = severity
            };

            _dispatcher.Dispatch(heartbeat);
        }
    }
}
