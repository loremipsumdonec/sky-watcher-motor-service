using Boilerplate.Features.Core.Commands;
using Sentry;

namespace Boilerplate.Features.Sentry.Commands
{
    public class SentryTransactionCommandDispatcherDecorator
        : ICommandDispatcher
    {
        private readonly ICommandDispatcher _decorated;

        public SentryTransactionCommandDispatcherDecorator(ICommandDispatcher decorated)
        {
            _decorated = decorated;
        }

        public async Task<bool> DispatchAsync(ICommand command)
        {
            var current = SentrySdk.GetSpan();

            if (current is ITransaction transaction)
            {
                transaction.Name = command.GetType().Name;
                transaction.Operation = "command";
            }
            else
            {
                current = null;
            }

            try
            {
                return await _decorated.DispatchAsync(command);
            }
            catch (Exception ex)
            {
                if (current is ITransaction)
                {
                    current.Status = SpanStatus.InternalError;
                    current.Finish(ex);
                }

                throw;
            }
            finally
            {
                if (current is ITransaction && !current.Status.HasValue)
                {
                    current.Finish(SpanStatus.Ok);
                }
            }
        }
    }
}
