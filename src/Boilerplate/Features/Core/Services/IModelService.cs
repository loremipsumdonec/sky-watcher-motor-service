namespace Boilerplate.Features.Core.Services
{
    public interface IModelService
    {
        Task<IModel> CreateModelAsync(object content);

        Task<T> CreateModelAsync<T>(object content = null) where T : IModel;
    }
}