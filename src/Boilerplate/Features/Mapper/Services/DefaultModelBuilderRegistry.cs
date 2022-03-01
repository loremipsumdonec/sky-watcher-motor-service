using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper.Attributes;

namespace Boilerplate.Features.Mapper.Services
{
    public class DefaultModelBuilderRegistry
        : IModelBuilderRegistry
    {
        private readonly IPrepareBuilderAttributes _prepareBuilderAttributes;
        private List<BuilderForAttribute> _registry;
        private readonly object _door = new object();
        private bool _isPrepared;

        public DefaultModelBuilderRegistry()
        {
            _registry = new List<BuilderForAttribute>();
            _prepareBuilderAttributes = new PrepareBuilderAttributes();
        }

        public void Add(BuilderForAttribute attribute)
        {
            if (!_isPrepared)
            {
                _registry.Add(attribute);
            }
        }

        public IEnumerable<Type> GetModelBuilders(object content, IModel model)
        {
            PrepareIfNotPrepared();
            List<Type> builders = new List<Type>();

            Type contentType = GetTypeFrom(content);

            BuilderForAttribute attribute = GetBuilderAttribute(contentType, model);

            if (attribute != null)
            {
                builders.AddRange(attribute.BuilderTypes);
            }

            return builders;
        }

        private BuilderForAttribute GetBuilderAttribute(
            Type contentType,
            IModel model)
        {
            var modelType = model.GetType();

            var attribute = _registry.Find(a =>
                a.ModelType == modelType &&
                a.WhenType == contentType
            );

            if (attribute == null)
            {
                attribute = _registry.Find(a =>
                                a.ModelType == modelType &&
                                a.WhenType.IsAssignableFrom(contentType));

                if (attribute == null)
                {
                    attribute = _registry.Find(a =>
                        a.ModelType.IsAssignableFrom(modelType) &&
                        a.WhenType.IsAssignableFrom(contentType));
                }
            }

            return attribute;
        }

        private Type GetTypeFrom(object obj)
        {
            Type type = typeof(object);

            if (obj != null)
            {
                type = obj.GetType();
            }

            return type;
        }

        private void PrepareIfNotPrepared()
        {
            if (!_isPrepared)
            {
                lock (_door)
                {
                    if (!_isPrepared)
                    {
                        _prepareBuilderAttributes.Prepare(_registry);
                        _isPrepared = true;
                    }
                }
            }
        }
    }
}
