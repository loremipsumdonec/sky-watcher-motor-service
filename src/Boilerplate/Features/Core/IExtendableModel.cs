namespace Boilerplate.Features.Core
{
    public interface IExtendableModel
        : IModel
    {
        Dictionary<string, object> Extensions { get; }

        void AddExtension(string key, object model);
    }
}
