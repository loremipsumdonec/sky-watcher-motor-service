
namespace Boilerplate.Features.Core
{
    public interface IModel
    {
    }

    public interface IModelWithType
        : IModel
    {
        string ModelType { get; }
    }
}