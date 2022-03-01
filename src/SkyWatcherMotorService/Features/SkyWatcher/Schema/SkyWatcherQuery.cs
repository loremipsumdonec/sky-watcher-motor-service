using Boilerplate.Features.Core.Queries;
using SkyWatcherMotorService.Features.SkyWatcher.Queries;

namespace SkyWatcherMotorService.Features.SkyWatcher.Schema
{
    public class SkyWatcherQuery
    {
        public Task<GetMountsModel> Mounts( [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetMountsModel>(
                new GetMounts()
            );
        }

        public Task<GetBreakStepsModel> MotorBreakSteps(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetBreakStepsModel>(
                new GetBreakSteps(motorId)
            );
        }

        public Task<GetCountsPerRevolutionModel> MotorCountsPerRevolution(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetCountsPerRevolutionModel>(
                new GetCountsPerRevolution(motorId)
            );
        }

        public Task<GetHighSpeedRatioModel> MotorHighSpeedRatio(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetHighSpeedRatioModel>(
                new GetHighSpeedRatio(motorId)
            );
        }

        public Task<GetInterruptFrequencyModel> MotorInterruptFrequency(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetInterruptFrequencyModel>(
                new GetInterruptFrequency(motorId)
            );
        }

        public Task<GetMotorBoardVersionModel> MotorBoardVersion(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetMotorBoardVersionModel>(
                new GetMotorBoardVersion(motorId)
            );
        }

        public Task<GetPositionModel> MotorPosition(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetPositionModel>(
                new GetPosition(motorId)
            );
        }

        public Task<GetStatusModel> MotorStatus(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetStatusModel>(
                new GetStatus(motorId)
            );
        }

        public Task<GetStepPeriodModel> MotorStepPeriod(string motorId, [Service] IQueryDispatcher dispatcher)
        {
            return dispatcher.DispatchAsync<GetStepPeriodModel>(
                new GetStepPeriod(motorId)
            );
        }
    }
}
