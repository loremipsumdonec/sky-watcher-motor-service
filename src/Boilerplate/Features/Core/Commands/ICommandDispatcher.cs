namespace Boilerplate.Features.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task<bool> DispatchAsync(ICommand command);
    }
}