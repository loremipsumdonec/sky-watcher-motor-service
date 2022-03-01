
using Boilerplate.Features.Core.Extensions;

namespace Boilerplate.Features.Core.NamingConventions
{
    public class DotNotation
        : DelimiterSeparatedCase
    {
        public override char Delimiter => '.';

        public override string To(string text)
        {
            return base.To(text.ToLowerFirstLetter());
        }

        protected override char TransformAfterDelimiter(char chr)
        {
            return char.ToLower(chr);
        }
    }
}
