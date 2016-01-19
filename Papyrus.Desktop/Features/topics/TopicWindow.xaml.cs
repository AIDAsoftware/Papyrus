using System;
using System.IO;
using System.Windows;
using Windows.UI.Notifications;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.DomainEvents;
using XmlDocument = Windows.Data.Xml.Dom.XmlDocument;
using XmlNodeList = Windows.Data.Xml.Dom.XmlNodeList;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class TopicWindow : Window
    {

        private const String APP_ID = "Microsoft.Samples.DesktopToastsSample";
        public TopicVM ViewModel
        {
            get { return (TopicVM)DataContext; }
        }

        public TopicWindow(EditableTopic topic)
        {
            InitializeComponent();
            
            DataContext = ViewModelsFactory.Topic(topic);
            this.Loaded += OnLoad;
            EventBus.AsObservable<OnTopicSaved>().Subscribe(Handle);
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }

        //TODO: should it return a task?
        private async void Handle(OnTopicSaved domainEvent) {
            // Get a toast XML template
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Fill in the text elements
            var stringElements = toastXml.GetElementsByTagName("text");
            for (int i = 0; i < stringElements.Length; i++) {
                stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
            }

            // Specify the absolute path to an image
            String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml);

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }
    }
}
