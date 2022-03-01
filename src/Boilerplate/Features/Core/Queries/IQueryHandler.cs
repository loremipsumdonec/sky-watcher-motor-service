using Boilerplate.Features.Core.Commands;

namespace Boilerplate.Features.Core.Queries
{
    public interface IQueryHandler
    {
        IHeartbeatDispatcher Dispatcher { get; set; }

        Task<IModel> ExecuteAsync(IQuery query);
    }

    public abstract class QueryHandler<Q>
        : IQueryHandler where Q : IQuery
    {
        public IHeartbeatDispatcher Dispatcher { get; set; }

        public Dictionary<string, object> Tags { get; set; }

        public Task<IModel> ExecuteAsync(IQuery query)
        {
            return ExecuteAsync((Q)query);
        }

        public abstract Task<IModel> ExecuteAsync(Q query);

        protected void Tag(string key, object value)
        {
            if (Tags != null)
            {
                if (Tags.ContainsKey(key))
                {
                    Tags[key] = value;
                }
                else
                {
                    Tags.Add(key, value);
                }
            }
        }

        protected void Debug(string message, object data = null)
        {
            Heartbeat(message, Severitys.Debug, data);
        }

        protected void Info(string message, object data = null)
        {
            Heartbeat(message, Severitys.Info, data);
        }

        protected void Warn(string message, object data = null)
        {
            Heartbeat(message, Severitys.Warn, data);
        }

        protected void Error(string message, object data = null)
        {
            Heartbeat(message, Severitys.Error, data);
        }

        protected void Fatal(string message, object data = null)
        {
            Heartbeat(message, Severitys.Fatal, data);
        }

        protected void Heartbeat(string message)
        {
            Heartbeat(message, Severitys.Progress);
        }

        protected void Heartbeat(string message, Severitys severity, object data = null)
        {
            Dispatcher?.Dispatch(message, severity, data);
        }
    }
}