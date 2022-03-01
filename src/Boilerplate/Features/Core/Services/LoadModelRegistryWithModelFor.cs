using Boilerplate.Features.Core.Attributes;
using System.Reflection;

namespace Boilerplate.Features.Core.Services
{
    public class LoadModelRegistryWithModelFor
        : IModelRegistry
    {
        private readonly IModelRegistry _decorated;
        private readonly IAssemblies _assemblies;
        private readonly object _door = new object();
        private bool _isLoaded;

        public LoadModelRegistryWithModelFor(IModelRegistry decorated)
            : this(decorated, null)
        {
        }

        public LoadModelRegistryWithModelFor(
            IModelRegistry decorated,
            IAssemblies assemblies)
        {
            _decorated = decorated;
            _assemblies = assemblies;
        }

        public void Add(Type type, Type modelType)
        {
            _decorated.Add(type, modelType);
        }

        public Type GetModelType(object obj)
        {
            LoadIfNotLoaded();
            return _decorated.GetModelType(obj);
        }

        public Type GetModelType(Type type)
        {
            LoadIfNotLoaded();
            return _decorated.GetModelType(type);
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
                                        .Where(t => typeof(IModel).IsAssignableFrom(t) && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes<ModelForAttribute>();

                        foreach (var attribute in attributes)
                        {
                            Add(attribute.Type, type);
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
