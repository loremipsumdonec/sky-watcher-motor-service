using Boilerplate.Features.Core;

namespace Boilerplate.Features.Mapper.Services
{
    public interface IModelBuilder
    {
        bool CanBuild(object content, IModel model);

        Task BuildAsync(object content, IModel model);
    }
}
