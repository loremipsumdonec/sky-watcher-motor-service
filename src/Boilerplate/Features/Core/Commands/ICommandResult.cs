namespace Boilerplate.Features.Core.Commands
{
    public interface ICommandResult
        : IModel
    {
        bool Status { get; }

        Exception Exception { get; }

        object Output { get; set; }

        Dictionary<string, object> Tags { get; }
    }
}