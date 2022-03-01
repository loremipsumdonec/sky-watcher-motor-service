using Autofac;
using Autofac.Core;
using Boilerplate.Features.Bulding.Builders;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Services;

namespace Boilerplate.Features.Mapper.Services
{
    public class BuildModelService
        : IModelService
    {
        private readonly IModelService _decorated;
        private readonly IModelBuilderRegistry _registry;
        private readonly ILifetimeScope _scope;

        public BuildModelService(
            IModelService decorated,
            IModelBuilderRegistry registry,
            ILifetimeScope scope)
        {
            _decorated = decorated;
            _registry = registry;
            _scope = scope;
        }

        public async Task<IModel> CreateModelAsync(object content)
        {
            IModel model = await _decorated.CreateModelAsync(content);
            return await BuildAsync(content, model);
        }

        public async Task<T> CreateModelAsync<T>(object content = null) where T : IModel
        {
            T model = await _decorated.CreateModelAsync<T>(content);
            return await BuildAsync(content, model);
        }

        private async Task<T> BuildAsync<T>(object content, T model) where T : IModel
        {
            return (T)await BuildModelAsync(content, model);
        }

        private async Task<IModel> BuildModelAsync(object content, IModel model)
        {
            if (model != null)
            {
                var builder = GetModelBuilder(content, model);

                if (builder != null)
                {
                    bool canBuild = builder.CanBuild(content, model);

                    if (canBuild)
                    {
                        await builder.BuildAsync(content, model);
                    }
                }
            }

            return model;
        }

        private IModelBuilder GetModelBuilder(object content, IModel model)
        {
            IEnumerable<Type> builders = _registry.GetModelBuilders(content, model);
            return CreateModelBuilder(builders);
        }

        private IModelBuilder CreateModelBuilder(IEnumerable<Type> builders, params object[] parameters)
        {
            if (builders.Count() > 1)
            {
                CompositeModelBuilder composite = new CompositeModelBuilder();

                foreach (var builderType in builders)
                {
                    composite.Add(
                        CreateModelBuilder(builderType, parameters)
                    );
                }

                return composite;
            }
            else if (builders.Count() == 1)
            {
                return CreateModelBuilder(builders.First(), parameters);
            }

            return null;
        }

        private IModelBuilder CreateModelBuilder(Type builderType, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return (IModelBuilder)_scope.Resolve(builderType);
            }

            List<Parameter> para = new List<Parameter>();

            foreach (var p in parameters)
            {
                para.Add(new TypedParameter(p.GetType(), p));
            }

            return (IModelBuilder)_scope.Resolve(builderType, para);
        }

    }
}
