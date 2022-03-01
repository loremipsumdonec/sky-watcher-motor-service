using Boilerplate.Features.Core.Commands;

namespace Boilerplate.Features.MassTransit.Services
{
    public interface IDistributedCommandDispatcher
    {
        public bool IsDistributed(ICommand command);

        public Task<bool> DispatchAsync(ICommand command);
    }
}
