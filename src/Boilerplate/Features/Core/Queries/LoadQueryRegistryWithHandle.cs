using System.Reflection;

namespace Boilerplate.Features.Core.Queries
{
    public class LoadQueryRegistryWithHandle
        : IQueryRegistry
    {
        private readonly IQueryRegistry _decorated;
        private readonly IAssemblies _assemblies;
        private readonly object _door = new object();
        private bool _isLoaded;

        public LoadQueryRegistryWithHandle(
            IQueryRegistry decorated, IAssemblies assemblies)
        {
            _decorated = decorated;
            _assemblies = assemblies;
        }

        public void Add(Type type, Type when)
        {
            _decorated.Add(type, when);
        }

        public Type GetHandler(IQuery query)
        {
            LoadIfNotLoaded();
            return _decorated.GetHandler(query);
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
                                        .Where(t => typeof(IQueryHandler).IsAssignableFrom(t) && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes<HandleAttribute>();

                        foreach (var attribute in attributes)
                        {
                            Add(type, attribute.MessageType);
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
