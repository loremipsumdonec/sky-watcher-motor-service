namespace Boilerplate.Features.Core.Commands
{
    public abstract class CommandHandler<T>
        : ICommandHandler where T : Command
    {
        private ICommand _command;

        public void Init(ICommand command)
        {
            _command = command;
            Init((T)command);
        }

        public IHeartbeatDispatcher Dispatcher { get; set; }

        public Dictionary<string, object> Tags { get; set; }

        public Task<bool> ExecuteAsync(ICommand command)
        {
            return ExecuteAsync((T)command);
        }

        public virtual void Init(T command)
        {
        }

        public abstract Task<bool> ExecuteAsync(T command);

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
