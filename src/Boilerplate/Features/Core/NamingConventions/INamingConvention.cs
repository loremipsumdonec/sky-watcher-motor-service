
namespace Boilerplate.Features.Core.NamingConventions
{
    public interface INamingConvention
    {
        string To(string text);

        string From(string text);

        string From(string text, INamingConvention to);
    }
}
