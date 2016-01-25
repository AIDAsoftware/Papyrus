using System;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Papyrus.Desktop.Features.Topics {
    public class ToastNotificator {
        private const String APP_ID = "Microsoft.Samples.DesktopToastsSample";

        public static void NotifyMessage(string message) {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            var stringElements = toastXml.GetElementsByTagName("text");
            stringElements[1].AppendChild(toastXml.CreateTextNode(message));

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }
    }
}