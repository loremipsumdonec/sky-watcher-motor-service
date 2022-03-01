using Boilerplate.Features.Core;
using Boilerplate.Features.Reactive.Events;
using System.Reflection;

namespace Boilerplate.Features.Reactive.Services
{
    public class LoadEventHandlerRegistryWithEventHandler
        : IEventHandlerRegistry
    {
        private readonly IEventHandlerRegistry _decorated;
        private readonly IAssemblies _assemblies;
        private readonly object _door = new object();
        private bool _isLoaded;

        public LoadEventHandlerRegistryWithEventHandler(
            IEventHandlerRegistry decorated,
            IAssemblies assemblies)
        {
            _decorated = decorated;
            _assemblies = assemblies;
        }

        public void Add(Type type)
        {
            _decorated.Add(type);
        }

        public void Enable(Type type)
        {
            _decorated.Enable(type);
        }

        public void Disable(Type type)
        {
            _decorated.Disable(type);
        }

        public IEnumerable<Type> GetHandlers()
        {
            LoadIfNotLoaded();
            return _decorated.GetHandlers();
        }

        public IEnumerable<Type> GetAllHandlers()
        {
            LoadIfNotLoaded();
            return _decorated.GetAllHandlers();
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
                                        .Where(t => typeof(IEventHandler).IsAssignableFrom(t) && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        Add(type);
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
