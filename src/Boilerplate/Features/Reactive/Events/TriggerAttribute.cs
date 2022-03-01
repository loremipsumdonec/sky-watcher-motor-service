namespace Boilerplate.Features.Reactive.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TriggerAttribute
        : Attribute
    {
        public TriggerAttribute(Type eventType)
        {
            EventType = eventType;
        }

        public Type MessageType { get; set; }

        public Type EventType { get; }
    }
}