using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using SkyWatcherMotorService.Features.SkyWatcher.Models;
using SkyWatcherMotorService.Features.SkyWatcher.Services;

namespace SkyWatcherMotorService.Features.SkyWatcher.Commands
{
    public class Slew
        : Command
    {
        public Slew(
            string motorId, 
            double degree, 
            bool tracking = false, 
            bool highSpeed = false)
        {
            MotorId = motorId;
            Degree = degree;
            Tracking = tracking;
            HighSpeed = highSpeed;
        }

        public string MotorId { get; set; }

        public double Degree { get; set; }

        public bool Tracking { get; set; }

        public bool HighSpeed { get; set; }
    }

    [Flags]
    public enum MotionMode : short
    {
        FastWhenTracking = 32,
        SlowWhenGoTo = 32,
        Tracking = 16,
        CounterClockWise = 1,
        None = 0
    }


    [Handle(typeof(Slew))]
    public class SlewHandler
        : CommandHandler<Slew>
    {
        private readonly ISkyWatcherManager _manager;

        public SlewHandler(ISkyWatcherManager manager)
        {
            _manager = manager;
        }

        public override async Task<bool> ExecuteAsync(Slew command)
        {
            var motor = _manager.GetMotor(command.MotorId);

            using (var client = _manager.CreateClient(motor))
            {
                long steps = motor.ConvertDegreeToSteps(command.Degree);
                var motionMode = GetMotionMode(command);

                await client.SendAsync("K", 0);
                await client.SendAsync("G", motionMode, 2);
                
                if(command.Tracking) 
                {
                    long t1Preset = GetT1Preset(steps, motor.TimerInterruptFrequency);
                    await client.SendAsync("I", t1Preset);
                } 
                else
                {
                    await client.SendAsync("H", steps);
                    await client.SendAsync("M", motor.BreakSteps);
                }

                await client.SendAsync("J", 0);
            }

            return true;
        }

        private long GetT1Preset(long steps, long timerInterruptFrequency) 
        {
            if(steps <= 0) 
            {
                return timerInterruptFrequency;
            }

            return timerInterruptFrequency/steps;
        }

        private long GetMotionMode(Slew command) 
        {
            MotionMode motionMode = MotionMode.None;

            if(command.Tracking) 
            {
                motionMode |= MotionMode.Tracking;
            }

            if(command.HighSpeed && command.Tracking) 
            {
                motionMode |= MotionMode.FastWhenTracking;
            }

            if(command.HighSpeed && !command.Tracking) 
            {
                motionMode |= MotionMode.SlowWhenGoTo; 
            }

            if(command.Degree < 0)
            {
                motionMode |= MotionMode.CounterClockWise;
            }

            return (long)motionMode;
        }
    }
}
