using System;
using System.ComponentModel;
using System.Windows;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.DomainEvents;
using XmlDocument = Windows.Data.Xml.Dom.XmlDocument;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class TopicWindow : Window
    {
        private const string ExitWithoutSaveMessage = "Esta apunto de salir del modo edición, ¿desea guardar los cambios realiados?";

        public TopicVM ViewModel
        {
            get { return (TopicVM)DataContext; }
        }

        public TopicWindow(EditableTopic topic)
        {
            InitializeComponent();
            
            DataContext = ViewModelsFactory.Topic(topic);
            this.Loaded += OnLoad;
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e) {
            if (ViewModel.TopicIsSaved()) return;
            var messageBoxResult = MessageBox.Show(ExitWithoutSaveMessage, "Aviso", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                ViewModel.SaveTopic.ExecuteAsync(null);
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
