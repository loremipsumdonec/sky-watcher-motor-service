namespace Boilerplate.Features.Core.Commands
{
    public class CommandResult
        : ICommandResult
    {
        public CommandResult()
        {
            Tags = new Dictionary<string, object>();
        }

        public bool Status { get; set; }

        public Exception Exception { get; set; }

        public object Output { get; set; }

        public Dictionary<string, object> Tags { get; set; }
    }
}