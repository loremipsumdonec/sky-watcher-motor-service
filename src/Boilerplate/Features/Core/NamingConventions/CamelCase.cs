using System.Text;

namespace Boilerplate.Features.Core.NamingConventions
{
    public class CamelCase
        : INamingConvention
    {
        public string From(string text)
        {
            StringBuilder builder = new StringBuilder();
            return builder.ToString();
        }

        public string From(string text, INamingConvention to)
        {
            string pascalCase = From(text);
            return to.To(pascalCase);
        }

        public virtual string To(string text)
        {
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < text.Length; index++)
            {
                char chr = text[index];

                if (index == 0)
                {
                    chr = char.ToLower(chr);
                }

                if (char.IsLetterOrDigit(chr))
                {
                    builder.Append(chr);
                }
            }

            return builder.ToString();
        }
    }
}