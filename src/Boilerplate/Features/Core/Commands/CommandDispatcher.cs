using Autofac;

namespace Boilerplate.Features.Core.Commands
{
    public class CommandDispatcher
        : ICommandDispatcher
    {
        private readonly ILifetimeScope _scope;
        private readonly ICommandRegistry _registry;

        public CommandDispatcher(ICommandRegistry registry, ILifetimeScope scope)
        {
            _registry = registry;
            _scope = scope;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            CommandResult result = new CommandResult();
            command.CommandResult = result;

            try
            {
                ICommandHandler handler = GetCommandHandler(command);
                handler.Tags = result.Tags;
                handler.Init(command);

                result.Status = await handler.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                throw;
            }

            return result.Status;
        }

        private ICommandHandler GetCommandHandler(ICommand command)
        {
            ICommandHandler handler = null;
            var handlerType = _registry.GetHandler(command);

            if (handlerType != null)
            {
                handler = (ICommandHandler)_scope.Resolve(handlerType);
                handler.Dispatcher = _scope.Resolve<IHeartbeatDispatcher>();
            }

            return handler;
        }
    }
}