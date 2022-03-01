
namespace Boilerplate.Features.Core.Commands
{
    public class DefaultHeartbeatDispatcher
        : IHeartbeatDispatcher
    {
        public void Dispatch(string message, Severitys severity, object data = null)
        {
        }
    }
}
