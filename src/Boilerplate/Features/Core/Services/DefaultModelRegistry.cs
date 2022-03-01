using Boilerplate.Features.Core.Attributes;

namespace Boilerplate.Features.Core.Services
{
    public class DefaultModelRegistry
        : IModelRegistry
    {
        private readonly List<ModelForAttribute> _registry;

        public DefaultModelRegistry()
        {
            _registry = new List<ModelForAttribute>();
        }

        public void Add(Type type, Type modelType)
        {
            _registry.Add(new ModelForAttribute(type, modelType));
        }

        public Type GetModelType(object obj)
        {
            return GetModelType(obj.GetType());
        }

        public Type GetModelType(Type type)
        {
            var attribute = _registry.Find(a => a.Type.Equals(type));

            if (attribute == null)
            {
                attribute = _registry.Find(a => a.Type.IsAssignableFrom(type));

                if (attribute == null)
                {
                    throw new InvalidOperationException($"Could not find a valid model type for {type}");
                }
            }

            return attribute.ModelType;
        }
    }
}
