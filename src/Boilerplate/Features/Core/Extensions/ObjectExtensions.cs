using System.ComponentModel;
using System.Globalization;

namespace Boilerplate.Features.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static TResult CastTo<TResult>(this object content) where TResult : class
        {
            return content as TResult;
        }

        public static TResult CastToType<TResult>(this object content)
        {
            if (content == null)
            {
                return default;
            }

            return (TResult)Convert.ChangeType(content, typeof(TResult), CultureInfo.InvariantCulture);
        }

        public static object CastToType(this object content, Type type)
        {
            return content == null
                ? default
                : TypeDescriptor.GetConverter(type).ConvertFromInvariantString(content.ToString());
        }
    }
}
