using Boilerplate.Features.Core.Queries;

namespace Boilerplate.Features.Core.Commands
{
    public interface ICommandRegistry
    {
        void Add(Type type, Type when);

        Type GetHandler(ICommand command);
    }
}
