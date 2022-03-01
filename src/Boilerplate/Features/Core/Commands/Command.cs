
namespace Boilerplate.Features.Core.Commands
{
    public abstract class Command
        : Message, ICommand
    {
        public ICommandResult CommandResult { get; set; }
    }
}