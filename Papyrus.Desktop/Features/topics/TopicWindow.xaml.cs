using System;
using System.Windows;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.DomainEvents;
using XmlDocument = Windows.Data.Xml.Dom.XmlDocument;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class TopicWindow : Window
    {
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

        // TODO: should it return a task?
        private async void Handle(OnTopicSaved domainEvent) {
            ToastNotificator.NotifyMessage("Topic Guardado");
        }
    }
}
