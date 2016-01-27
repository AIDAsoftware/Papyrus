using Windows.UI.Notifications;

namespace Papyrus.Desktop.Features.Topics {
    public class ToastNotificator {
        private const string AppId = "Microsoft.Samples.DesktopToastsSample";

        public static void NotifyMessage(string message) {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            var stringElements = toastXml.GetElementsByTagName("text");
            stringElements[1].AppendChild(toastXml.CreateTextNode(message));

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
        }
    }
}