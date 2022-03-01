using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using MassTransit;

namespace Boilerplate.Features.MassTransit.Services
{
        public class CommandConsumer<T>
        : IConsumer<T> where T : class, ICommand
    {
        private readonly ICommandDispatcher _dispatcher;

        public CommandConsumer(ICommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            var status = await _dispatcher.DispatchAsync(context.Message);
            
            await context.RespondAsync(
                context.Message.CommandResult, 
                context.Message.CommandResult.GetType()
            );
        }
    }
}
