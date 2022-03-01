using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper.Services;

namespace Boilerplate.Features.Bulding.Builders
{
    public class CompositeModelBuilder
        : IModelBuilder
    {
        private readonly List<IModelBuilder> _builders;

        public CompositeModelBuilder()
        {
            _builders = new List<IModelBuilder>();
        }

        public bool CanBuild(object content, IModel model)
        {
            return true;
        }

        public async Task BuildAsync(object content, IModel model)
        {
            foreach (var builder in _builders)
            {
                try
                {
                    if (builder.CanBuild(content, model))
                    {
                        await builder.BuildAsync(content, model);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Add(IModelBuilder builder)
        {
            _builders.Add(builder);
        }
    }
}
