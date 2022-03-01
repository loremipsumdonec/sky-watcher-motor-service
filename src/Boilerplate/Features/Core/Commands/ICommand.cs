
namespace Boilerplate.Features.Core.Commands
{
    public interface ICommand
        : IMessage
    {
        ICommandResult CommandResult { get; set; }
    }
}