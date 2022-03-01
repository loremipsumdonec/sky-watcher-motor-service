using Boilerplate.Features.Core.Queries;

namespace Boilerplate.Features.Core.Commands
{
    public class DefaultCommandRegistry
        : ICommandRegistry
    {
        private readonly List<HandleAttribute> _registry;

        public DefaultCommandRegistry()
        {
            _registry = new List<HandleAttribute>();
        }

        public void Add(Type type, Type when)
        {
            _registry.Add(new HandleAttribute(when)
            {
                HandlerType = type
            });
        }

        public Type GetHandler(ICommand command)
        {
            var exists = _registry.Find(a => a.MessageType.IsAssignableFrom(command.GetType()));

            if (exists != null)
            {
                return exists.HandlerType;
            }

            return null;
        }
    }
}
