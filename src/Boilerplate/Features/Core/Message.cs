namespace Boilerplate.Features.Core
{
    public abstract class Message
        : IMessage
    {
        private Guid _messageId;

        public string TraceParent { get; set; }

        public Guid MessageId
        {
            get
            {
                if (_messageId == Guid.Empty)
                {
                    _messageId = Guid.NewGuid();
                }

                return _messageId;
            }
            set
            {
                _messageId = value;
            }
        }

        public Guid ParentMessageId { get; set; }
    }
}