using System;
using System.Windows;
using Windows.UI.Notifications;

namespace Papyrus.Desktop.Features.Topics {
    public class ToastNotificator {
        private const string AppId = "Microsoft.Samples.DesktopToastsSample";

        public static void NotifyMessage(string message) {
            if (IsModernOperatingSystem()) {
                ShowMessageForModernOS(message);    
            }
            else {
                ShowMessageForLegacyOS(message);
            }
        }

        private static bool IsModernOperatingSystem() {
            var windows8Version = new Version(6,2);
            return Environment.OSVersion.Version >= windows8Version;
        }

        private static void ShowMessageForLegacyOS(string message) {
            MessageBox.Show(message);
        }

        private static void ShowMessageForModernOS(string message) {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            var stringElements = toastXml.GetElementsByTagName("text");
            stringElements[1].AppendChild(toastXml.CreateTextNode(message));

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier(AppId).Show(toast);
        }
    }
}