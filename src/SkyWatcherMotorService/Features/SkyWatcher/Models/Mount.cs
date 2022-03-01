using Boilerplate.Features.Core;

namespace SkyWatcherMotorService.Features.SkyWatcher.Models
{
    public class Mount
        : IModel
    {
        public Mount()
        {
        }

        public Mount(string mountId)
        {
            MountId = mountId;
        }

        public bool Connected { get; set; }

        public string MountId { get; set; }

        public IEnumerable<Motor> Motors { get; set; } = new List<Motor>();

        public void Add(Motor motor) 
        {
            ((List<Motor>)Motors).Add(motor);
        }
    }
}
