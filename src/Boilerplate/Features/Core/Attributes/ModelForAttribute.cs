namespace Boilerplate.Features.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ModelForAttribute
        : Attribute
    {
        public ModelForAttribute(Type type)
        {
            Type = type;
        }

        public ModelForAttribute(Type type, Type modelType)
        {
            Type = type;
            ModelType = modelType;
        }

        public Type Type { get; }

        public Type ModelType { get; set; }

        public override string ToString()
        {
            return $"{Type} = {ModelType}";
        }
    }
}