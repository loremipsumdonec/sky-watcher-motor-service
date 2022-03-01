namespace Boilerplate.Features.Core.Commands
{
    public interface ICommandHeartbeatHandler
    {
        void Heartbeat(string message, Severitys severity, object data = null);
    }
}
