using System.Reflection;

namespace Boilerplate.Features.Core.Services
{
    public class Assemblies
        : IAssemblies
    {
        private readonly object _door = new object();
        private readonly IEnumerable<string> _names;
        private List<Assembly> _assemblies;

        public Assemblies(IEnumerable<string> names)
        {
            _names = names;
        }

        public Assemblies(IEnumerable<Assembly> assemblies)
        {
            _assemblies = new List<Assembly>(assemblies);
        }

        public List<Assembly> Get()
        {
            lock (_door)
            {
                if (_assemblies == null)
                {
                    _assemblies = GetAssemblies();
                }
            }

            return new List<Assembly>(_assemblies);
        }

        private List<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();

            foreach (string name in _names)
            {
                var n = new AssemblyName(name);
                assemblies.Add(Assembly.Load(n));
            }

            return assemblies;
        }
    }
}