using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Models;
using SkyWatcherMotorService.Features.SkyWatcher.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Commands
{
    public class GoTo
        : Command
    {
        public GoTo(string mountId, double azimuth, double altitude)
        {
            MountId = mountId;
            Azimuth = azimuth;
            Altitude = altitude;
        }

        public string MountId { get; set; }

        public double Azimuth { get; set; }

        public double Altitude { get; set; }
    }

    [Handle(typeof(GoTo))]
    public class GoToHandler
        : CommandHandler<GoTo>
    {
        private readonly ISkyWatcherManager _manager;
        private readonly IQueryDispatcher _dispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public GoToHandler(
            ISkyWatcherManager manager, 
            IQueryDispatcher dispatcher, 
            ICommandDispatcher commandDispatcher
        )
        {
            _manager = manager;
            _dispatcher = dispatcher;
            _commandDispatcher = commandDispatcher;
        }

        public override async Task<bool> ExecuteAsync(GoTo command)
        {
            var azimuthMotor = _manager.GetMotor($"{command.MountId}/1");
            var altitudeMotor = _manager.GetMotor($"{command.MountId}/2");

            await GoToAzimuthAsync(azimuthMotor, command);
            await GoToAltitudeAsync(altitudeMotor, command);

            return true;
        }

        private async Task GoToAzimuthAsync(Motor motor, GoTo command)
        {
            var position = await _dispatcher.DispatchAsync<GetPositionModel>(
                new GetPosition(motor.MotorId)
            );

            if(Math.Round(position.Degree) == command.Azimuth) 
            {
                return;
            }

            double angle = GetSmallestAngle(position.Degree, command.Azimuth);

            await _commandDispatcher.DispatchAsync(
                new Slew(motor.MotorId, angle)
            );
        }

        private async Task GoToAltitudeAsync(Motor motor, GoTo command) 
        {
            var position = await _dispatcher.DispatchAsync<GetPositionModel>(
                new GetPosition(motor.MotorId)
            );

            if(Math.Round(position.Degree) == command.Altitude) 
            {
                return;
            }

            double angle = GetSmallestAngle(position.Degree, command.Altitude);

            await _commandDispatcher.DispatchAsync(
                new Slew(motor.MotorId, angle)
            );
        }

        private double GetSmallestAngle(double current, double target) 
        {
            double distance = (target - current) % 360;
            return distance <= 180 ? distance : -(360 - distance);
        }
    }
}
