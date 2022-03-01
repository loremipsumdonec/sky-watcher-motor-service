using Boilerplate.Features.Core;
using Boilerplate.Features.Mapper.Attributes;
using System.Reflection;

namespace Boilerplate.Features.Mapper.Services
{
    public class LoadModelBuilderRegistryWithBuilderFor
        : IModelBuilderRegistry
    {
        private readonly IModelBuilderRegistry _decorated;
        private readonly IAssemblies _assemblies;
        private readonly object _door = new object();
        private bool _isLoaded;

        public LoadModelBuilderRegistryWithBuilderFor(
            IModelBuilderRegistry decorated,
            IAssemblies assemblies)
        {
            _decorated = decorated;
            _assemblies = assemblies;
        }

        public void Add(BuilderForAttribute attribute)
        {
            _decorated.Add(attribute);
        }

        public IEnumerable<Type> GetModelBuilders(object content, IModel model)
        {
            LoadIfNotLoaded();
            return _decorated.GetModelBuilders(content, model);
        }

        private void LoadIfNotLoaded()
        {
            if (!_isLoaded)
            {
                lock (_door)
                {
                    if (!_isLoaded)
                    {
                        ForceLoad();
                        _isLoaded = true;
                    }
                }
            }
        }

        private void ForceLoad()
        {
            foreach (Assembly assembly in _assemblies.Get())
            {
                try
                {
                    var types = assembly.GetExportedTypes()
                                        .Where(t => typeof(IModelBuilder).IsAssignableFrom(t) && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes<BuilderForAttribute>();

                        foreach (var attribute in attributes)
                        {
                            attribute.BuilderType = type;
                            Add(attribute);
                        }
                    }
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}
