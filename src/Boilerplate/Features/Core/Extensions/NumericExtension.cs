
namespace Boilerplate.Features.Core.Extensions
{
    public static class NumericExtension
    {
        public static int Approximation(this int value, int digits)
        {
            return (int)((double)value).Approximation(digits);
        }

        public static long Approximation(this long value, int digits)
        {
            return (long)((double)value).Approximation(digits);
        }

        public static double Approximation(this double value, int digits)
        {
            int counter = 0;
            int modifier = 1;
            if (value == 0)
            {
                return 0;
            }
            if (value < 0)
            {
                modifier = -1;
            }
            value *= modifier;
            while (value < System.Math.Pow(10, digits - 1))
            {
                value *= 10;
                counter--;
            }
            while (value >= System.Math.Pow(10, digits))
            {
                value /= 10;
                counter++;
            }
            value = System.Math.Floor(value);
            double power = System.Math.Pow(10, counter);
            return value * power * modifier;
        }

        public static bool Between(this int num, int lower, int upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

        public static bool Between(this double num, int lower, int upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }
    }
}
