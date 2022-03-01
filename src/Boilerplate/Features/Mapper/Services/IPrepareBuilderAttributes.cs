using Boilerplate.Features.Mapper.Attributes;

namespace Boilerplate.Features.Mapper.Services
{
    public interface IPrepareBuilderAttributes
    {
        void Prepare(List<BuilderForAttribute> attributes);
    }
}
