using Boilerplate.Features.Core.NamingConventions;
using System.Globalization;

namespace Boilerplate.Features.Core.Extensions
{
    public static class StringExtension
    {
        public static string TruncateWords(this string value, int words)
        {
            return string.Join(" ",
                value.Split(' ').Take(words)
            );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static object GetAs(this string value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return value.GetAs(Nullable.GetUnderlyingType(type));
            }

            if (type == typeof(int))
            {
                try
                {
                    return int.Parse(value);
                }
                catch
                {
                    return 0;
                }
            }
            else if (type == typeof(double))
            {
                try
                {
                    value = value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                    value = value.Replace(",", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                    return double.Parse(value);
                }
                catch
                {
                    return 0.0;
                }
            }
            else if (type == typeof(DateTime))
            {
                try
                {
                    return DateTime.Parse(value);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            else if (type == typeof(bool))
            {
                try
                {
                    return bool.Parse(value);
                }
                catch
                {
                    return false;
                }
            }

            return value;
        }

        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);

            return new string(letters);
        }

        public static string ToLowerFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var letters = source.ToCharArray();
            letters[0] = char.ToLower(letters[0]);

            return new string(letters);
        }

        public static string ToCamelCase(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            if (source.Contains("."))
            {
                return new DotNotation().From(source, new CamelCase());
            }
            else if (source.Contains("-"))
            {
                return new KebabCase().From(source, new CamelCase());
            }
            else if (source.Contains("_"))
            {
                return new SnakeCase().From(source, new CamelCase());
            }

            return new CamelCase().To(source);
        }

        public static T GetAs<T>(this string value)
        {
            return (T)value.GetAs(typeof(T));
        }

        public static int GetSentenceCount(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return 0;
            }

            return data.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
