namespace SkyWatcherMotorService.Features.SkyWatcher.Models
{
    public class Motor
    {
        private double _factorRadianTopStep;
        private double _factorStepToRadian;

        public Motor()
        {
        }

        public Motor(string motorId)
        {
            MotorId = motorId;
        }

        public string MotorId { get; set; }

        public string Version { get; set; }

        public long CountsPerRevolution { get; set; }

        public double FactorRadianToStep
        { 
            get 
            {
                if(_factorRadianTopStep == 0 && CountsPerRevolution > 0) 
                {
                    _factorRadianTopStep = CountsPerRevolution / (2 * Math.PI);
                }

                return _factorRadianTopStep;
            }
        }

        public double FactorStepToRadian
        {
            get
            {
                if (_factorStepToRadian == 0 && CountsPerRevolution > 0)
                {
                    _factorStepToRadian = 2 * Math.PI / CountsPerRevolution;
                }

                return _factorStepToRadian;
            }
        }

        public long TimerInterruptFrequency { get; set; }

        public long StepPeriod { get; set; }

        public long HighSpeedRatio { get; set; }

        public long BreakSteps { get; set; }

        public long ConvertDegreeToSteps(double degree) 
        {
            if(degree < 0)
            {
                degree *= -1;
            }

            double target = degree * Math.PI / 180.0;
            return (long)(target * FactorRadianToStep);
        }

        public double ConvertStepsToDegreee(long steps) 
        {
            double radian = steps * FactorStepToRadian;
            double degree = radian * 180.0 / Math.PI % 360;
        
            if(degree < 0)
            {
                return 360 - (degree * -1);
            }

            return degree;
        }
    }   
}
