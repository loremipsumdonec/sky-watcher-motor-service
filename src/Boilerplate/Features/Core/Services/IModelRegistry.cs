namespace Boilerplate.Features.Core.Services
{
    public interface IModelRegistry
    {
        void Add(Type type, Type modelType);

        Type GetModelType(object obj);

        Type GetModelType(Type type);
    }
}
