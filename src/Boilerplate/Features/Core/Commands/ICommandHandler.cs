namespace Boilerplate.Features.Core.Commands
{
    public interface ICommandHandler
    {
        IHeartbeatDispatcher Dispatcher { get; set; }

        Dictionary<string, object> Tags { get; set; }

        void Init(ICommand command);

        Task<bool> ExecuteAsync(ICommand command);
    }
}