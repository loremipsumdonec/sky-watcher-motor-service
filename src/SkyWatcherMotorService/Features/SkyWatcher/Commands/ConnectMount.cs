using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Models;
using SkyWatcherMotorService.Features.SkyWatcher.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Commands
{
    public class ConnectMount
        : Command
    {
        public ConnectMount(string mountId)
        {
            MountId = mountId;
        }

        public string MountId { get; set; }
    }

    [Handle(typeof(ConnectMount))]
    public class ConnectMotorHandler
        : CommandHandler<ConnectMount>
    {
        private readonly ISkyWatcherManager _manager;
        private readonly IQueryDispatcher _dispatcher;

        public ConnectMotorHandler(ISkyWatcherManager manager, IQueryDispatcher dispatcher)
        {
            _manager = manager;
            _dispatcher = dispatcher;
        }

        public override async Task<bool> ExecuteAsync(ConnectMount command)
        {
            var mount = _manager.CreateMount(command.MountId);
            await LoadMotorsAsync(mount);

            mount.Connected = true;

            return true;
        }

        private async Task LoadMotorsAsync(Mount mount)
        {
            for(int index = 1; index <= 2; index++) 
            {
                var motor = _manager.CreateMotor(index, mount);

                await LoadVersionAsync(motor);
                await LoadCountsPerRevolution(motor);
                await LoadHighSpeedRatio(motor);
                await LoadStepPeriod(motor);
                await LoadInterruptFrequency(motor);
                await LoadBreakSteps(motor);

                await MarkInitializationDone(motor);
            }
        }

        private async Task LoadVersionAsync(Motor motor) 
        {
            var model = await _dispatcher.DispatchAsync<GetMotorBoardVersionModel>(
                new GetMotorBoardVersion(motor.MotorId)
            );

            motor.Version = model.Version;
        }

        private async Task LoadCountsPerRevolution(Motor motor) 
        {
            var model = await _dispatcher.DispatchAsync<GetCountsPerRevolutionModel>(
                new GetCountsPerRevolution(motor.MotorId)
            );

            motor.CountsPerRevolution = model.Steps;
        }

        private async Task LoadHighSpeedRatio(Motor motor)
        {
            var model = await _dispatcher.DispatchAsync<GetHighSpeedRatioModel>(
                new GetHighSpeedRatio(motor.MotorId)
            );

            motor.HighSpeedRatio = model.Steps;
        }

        private async Task LoadStepPeriod(Motor motor)
        {
            var model = await _dispatcher.DispatchAsync<GetStepPeriodModel>(
                new GetStepPeriod(motor.MotorId)
            );

            motor.StepPeriod = model.Steps;
        }

        private async Task LoadInterruptFrequency(Motor motor)
        {
            var model = await _dispatcher.DispatchAsync<GetInterruptFrequencyModel>(
                new GetInterruptFrequency(motor.MotorId)
            );

            motor.TimerInterruptFrequency = model.Steps;
        }

        private async Task LoadBreakSteps(Motor motor)
        {
            var model = await _dispatcher.DispatchAsync<GetBreakStepsModel>(
                new GetBreakSteps(motor.MotorId)
            );

            motor.BreakSteps = model.Steps;
        }

        private async Task MarkInitializationDone(Motor motor) 
        {
            using(var client = _manager.CreateClient(motor)) 
            {
                await client.SendAsync("F", 0);
            }
        }
    }
}
