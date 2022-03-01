using System.Reflection;

namespace Boilerplate.Features.Core.Extensions
{
    public static class TypeExtension
    {
        internal static T GetCustomAttribute<T>(this Type reflectionSource) where T : class
        {
            var result = Activator.CreateInstance<T>();

            var attribute =
                CustomAttributeData.GetCustomAttributes(reflectionSource)
                    .FirstOrDefault(c => c.AttributeType.FullName == typeof(T).FullName);
            if (attribute == null)
            {
                return result;
            }

            if (attribute.ConstructorArguments.NullSafe().Any())
            {
                result = (T)Activator.CreateInstance(typeof(T), attribute.ConstructorArguments.Select(c => c.Value).ToArray());
            }
            foreach (var property in attribute.NamedArguments.NullSafe())
            {
                var resultProperty = result.GetType().GetProperty(property.MemberName, BindingFlags.Public | BindingFlags.Instance);
                if (resultProperty != null && resultProperty.CanWrite)
                {
                    resultProperty.SetValue(result, property.TypedValue.Value, null);
                }
            }

            return result;
        }

        public static bool IsDefined<T>(this Type source) where T : Attribute
        {
            return CustomAttributeData.GetCustomAttributes(source).Any(c => c.AttributeType.FullName == typeof(T).FullName);
        }

        internal static bool IsDefined<T>(this Assembly source) where T : Attribute
        {
            return CustomAttributeData.GetCustomAttributes(source).Any(c => c.AttributeType.FullName == typeof(T).FullName);
        }
    }
}
