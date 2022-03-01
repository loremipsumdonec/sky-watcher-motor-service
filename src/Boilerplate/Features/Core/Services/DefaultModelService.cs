
namespace Boilerplate.Features.Core.Services
{
    public class DefaultModelService
        : IModelService
    {
        private readonly IModelRegistry _registry;

        public DefaultModelService(IModelRegistry registry)
        {
            _registry = registry;
        }

        public Task<IModel> CreateModelAsync(object content = null)
        {
            if (content == null)
            {
                return null;
            }

            Type type = _registry.GetModelType(content);
            return Task.FromResult(CreateModel(content, type));
        }

        public Task<T> CreateModelAsync<T>(object content = null) where T : IModel
        {
            return Task.FromResult((T)CreateModel(content, typeof(T)));
        }

        private IModel CreateModel(object content, Type type)
        {
            IModel model;
            try
            {
                var constructor = type.GetConstructor(new Type[] { content.GetType() });

                if (constructor != null)
                {
                    model = (IModel)Activator.CreateInstance(type, new object[] { content });
                }
                else
                {
                    model = (IModel)Activator.CreateInstance(type);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed creating model with type {type} for {content.GetType()}", ex);
            }

            return model;
        }

    }
}
