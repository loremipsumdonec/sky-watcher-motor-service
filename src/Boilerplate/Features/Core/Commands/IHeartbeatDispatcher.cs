namespace Boilerplate.Features.Core.Commands
{
    public interface IHeartbeatDispatcher
    {
        void Dispatch(string message, Severitys severity, object data = null);
    }
}
