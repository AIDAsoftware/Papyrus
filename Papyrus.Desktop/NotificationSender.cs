using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop {
    public class NotificationSender {
        public virtual void SendNotification(string message) {
            EventBus.Send(new OnUserMessageRequest(message));
        }
    }
}