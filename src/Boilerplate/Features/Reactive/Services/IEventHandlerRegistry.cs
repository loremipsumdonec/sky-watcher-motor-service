namespace Boilerplate.Features.Reactive.Services
{
    public interface IEventHandlerRegistry
    {
        void Add(Type type);

        void Enable(Type handlerType);

        void Disable(Type handlerType);

        IEnumerable<Type> GetHandlers();

        IEnumerable<Type> GetAllHandlers();
    }
}
