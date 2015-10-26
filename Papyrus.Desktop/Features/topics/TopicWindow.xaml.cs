using System.Windows;
using Papyrus.Business.Topics;

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
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
