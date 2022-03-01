namespace Boilerplate.Features.Core.Queries
{

    public class DefaultQueryRegistry
        : IQueryRegistry
    {
        private readonly List<HandleAttribute> _registry;

        public DefaultQueryRegistry()
        {
            _registry = new List<HandleAttribute>();
        }

        public void Add(Type type, Type when)
        {
            _registry.Add(new HandleAttribute(when)
            {
                HandlerType = type
            });
        }

        public Type GetHandler(IQuery query)
        {
            var exists = _registry.Find(a => a.MessageType.IsAssignableFrom(query.GetType()));

            if (exists != null)
            {
                return exists.HandlerType;
            }

            return null;
        }
    }
}
