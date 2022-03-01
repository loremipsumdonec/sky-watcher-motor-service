using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Commands
{
    public class Stop
        : Command
    {
        public Stop(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }
    }

    [Handle(typeof(Stop))]
    public class StopHandler
        : CommandHandler<Stop>
    {
        private readonly ISkyWatcherManager _manager;

        public StopHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<bool> ExecuteAsync(Stop command)
        {
            var motor = _manager.GetMotor(command.MotorId);

            using (var client = _manager.CreateClient(motor))
            {
                await client.SendAsync("K", 0);
            }

            return true;
        }
    }
}
