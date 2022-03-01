namespace Boilerplate.Features.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> NullSafe<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable != null)
            {
                foreach (T current in enumerable)
                {
                    yield return current;
                }
            }
        }
    }
}
