
namespace Boilerplate.Features.Core.NamingConventions
{
    public class SnakeCase
        : DelimiterSeparatedCase
    {
        public override char Delimiter => '_';

        public override string To(string text)
        {
            return base.To(text).ToLower();
        }
    }
}
