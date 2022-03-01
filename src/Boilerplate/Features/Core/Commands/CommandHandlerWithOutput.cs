namespace Boilerplate.Features.Core.Commands
{
    public abstract class CommandHandlerWithOutput<T, O>
        : CommandHandler<T> where T : Command
    {
        public override async Task<bool> ExecuteAsync(T command)
        {
            command.CommandResult.Output = await ExecuteWithOutputAsync(command);
            return command.CommandResult.Output != null;
        }

        public abstract Task<O> ExecuteWithOutputAsync(T command);
    }
}