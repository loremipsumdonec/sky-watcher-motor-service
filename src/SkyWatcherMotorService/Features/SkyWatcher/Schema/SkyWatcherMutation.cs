using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Commands;
using SkyWatcherMotorService.Features.SkyWatcher.Models;
using SkyWatcherMotorService.Features.SkyWatcher.Queries;

namespace SkyWatcherMotorService.Features.SkyWatcher.Schema
{
    public class SkyWatcherMutation
    {
        public async Task<Mount> ConnectMount(string mountId, [Service] ICommandDispatcher dispatcher, [Service] IQueryDispatcher queryDispatcher)
        {
            await dispatcher.DispatchAsync(
                new ConnectMount(mountId)
            );

            return await queryDispatcher.DispatchAsync<Mount>(
                new GetMount(mountId)
            );
        }

        public Task<bool> GoTo(string mountId, double azimuth, double altitude, [Service] ICommandDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync(
                new GoTo(mountId, azimuth, altitude)
            );
        }

        public Task<bool> StopMotor(string motorId, [Service] ICommandDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync(
                new Stop(motorId)
            );
        }

        public Task<bool> SlewMotor(
            string motorId, 
            double degree, 
            bool tracking, 
            bool highSpeed, [Service] ICommandDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync(
                new Slew(motorId, degree, tracking, highSpeed)
            );
        }
    }
}
