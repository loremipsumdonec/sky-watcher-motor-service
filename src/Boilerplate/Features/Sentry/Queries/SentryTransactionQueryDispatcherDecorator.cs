using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Sentry;

namespace Boilerplate.Features.Sentry.Queries
{
    public class SentryTransactionQueryDispatcherDecorator
        : IQueryDispatcher
    {
        private readonly IQueryDispatcher _decorated;

        public SentryTransactionQueryDispatcherDecorator(IQueryDispatcher decorated)
        {
            _decorated = decorated;
        }

        public async Task<M> DispatchAsync<M>(IQuery query) where M : class, IModel
        {
            var current = SentrySdk.GetSpan();

            if (current is ITransaction transaction)
            {
                transaction.Name = query.GetType().Name;
                transaction.Operation = "query";
            }
            else
            {
                current = null;
            }

            try
            {
                return await _decorated.DispatchAsync<M>(query);
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

        public async Task<IModel> DispatchAsync(IQuery query)
        {
            var current = SentrySdk.GetSpan();

            if (current is ITransaction transaction)
            {
                transaction.Name = query.GetType().Name;
                transaction.Operation = "query";
            }
            else
            {
                current = null;
            }

            try
            {
                return await _decorated.DispatchAsync(query);
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
