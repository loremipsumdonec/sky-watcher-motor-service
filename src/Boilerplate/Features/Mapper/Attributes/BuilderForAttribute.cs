namespace Boilerplate.Features.Mapper.Attributes
{
    public enum BuildSequencePosition
    {
        Unknown,
        First,
        Last,
        Before,
        After
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class BuilderForAttribute
        : Attribute
    {
        public BuilderForAttribute(Type modelType)
            : this(modelType, typeof(object))
        {
        }

        public BuilderForAttribute(Type modelType, Type whenType)
        {
            ModelType = modelType;
            WhenType = whenType;
            UseInherit = false;
            BuilderTypes = new List<Type>();
            PositionInBuildSequence = BuildSequencePosition.Unknown;
        }

        public Type BuilderType { get; set; }

        public List<Type> BuilderTypes { get; set; }

        public Type ModelType { get; }

        public Type Dependency { get; set; }

        public BuildSequencePosition PositionInBuildSequence { get; set; }

        public bool UseInherit { get; set; }

        public Type WhenType { get; set; }

        public override string ToString()
        {
            return $"{BuilderType}, {ModelType}";
        }

        public bool IsDependentOf(BuilderForAttribute attribute)
        {
            if (Dependency == null && attribute.Dependency == null)
            {
                return false;
            }

            return Dependency?.Equals(attribute.BuilderType) == true;
        }
    }
}