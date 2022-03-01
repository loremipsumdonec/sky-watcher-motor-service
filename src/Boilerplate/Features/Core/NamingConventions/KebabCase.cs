
namespace Boilerplate.Features.Core.NamingConventions
{
    public class KebabCase
        : DelimiterSeparatedCase
    {
        public override char Delimiter => '-';

        public override string To(string text)
        {
            return base.To(text).ToLower();
        }
    }
}
