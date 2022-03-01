using System.Text;

namespace Boilerplate.Features.Core.NamingConventions
{
    public abstract class DelimiterSeparatedCase
        : INamingConvention
    {
        public abstract char Delimiter { get; }

        public string From(string text)
        {
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < text.Length; index++)
            {
                char chr = text[index];

                if (index == 0 || text[index - 1].Equals(Delimiter))
                {
                    builder.Append(char.ToUpper(chr));
                }
                else if (!chr.Equals(Delimiter))
                {
                    builder.Append(chr);
                }
            }

            return builder.ToString();
        }

        public virtual string From(string text, INamingConvention to)
        {
            string pascalCase = From(text);
            return to.To(pascalCase);
        }

        protected virtual char TransformAfterDelimiter(char chr)
        {
            return chr;
        }

        public virtual string To(string text)
        {
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < text.Length; index++)
            {
                char chr = text[index];

                if (index > 0
                    && (char.IsUpper(chr) || !char.IsLetterOrDigit(chr))
                    && builder.Length > (index - 1)
                    && !builder[index - 1].Equals(Delimiter))
                {
                    builder.Append(Delimiter);
                    chr = TransformAfterDelimiter(chr);
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
