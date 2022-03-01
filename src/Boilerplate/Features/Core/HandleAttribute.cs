namespace Boilerplate.Features.Core
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HandleAttribute
        : Attribute
    {
        public HandleAttribute(Type messageType)
        {
            MessageType = messageType;
        }

        public Type HandlerType { get; set; }

        public Type MessageType { get; }
    }
}