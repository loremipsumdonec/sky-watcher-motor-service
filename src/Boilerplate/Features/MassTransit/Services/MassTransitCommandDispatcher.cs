using Boilerplate.Features.Core.Commands;

namespace Boilerplate.Features.MassTransit.Services
{
    public class MassTransitCommandDispatcher
        : ICommandDispatcher
    {
        private readonly ICommandDispatcher _decorated;
        private readonly IDistributedCommandDispatcher _dispatcher;

        public MassTransitCommandDispatcher(ICommandDispatcher decorated, IDistributedCommandDispatcher dispatcher)
        {
            _decorated = decorated;
            _dispatcher = dispatcher;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            if (_dispatcher.IsDistributed(command))
            {
                return await _dispatcher.DispatchAsync(command);
            }

            return await _decorated.DispatchAsync(command);
        }
    }
}
