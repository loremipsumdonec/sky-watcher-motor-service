namespace Boilerplate.Features.Core.Queries
{
    public interface IQueryRegistry
    {
        void Add(Type type, Type when);

        Type GetHandler(IQuery query);
    }
}
