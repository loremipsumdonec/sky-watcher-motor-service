using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper.Attributes;

namespace Boilerplate.Features.Mapper.Services
{
    public interface IModelBuilderRegistry
    {
        void Add(BuilderForAttribute attribute);

        IEnumerable<Type> GetModelBuilders(object content, IModel model);
    }
}
