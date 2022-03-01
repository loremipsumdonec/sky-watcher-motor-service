using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyWatcherMotorServiceTest.Utility
{
    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int min, int max)
        {
            Random random = new Random();
            int count = random.Next(min, max);

            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(_ => Guid.NewGuid());
        }
    }
}
