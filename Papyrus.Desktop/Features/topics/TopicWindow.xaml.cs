using System.Windows;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class TopicWindow : Window
    {
        public TopicVM ViewModel
        {
            get { return (TopicVM)DataContext; }
        }

        public TopicWindow()
        {
            InitializeComponent();

            DataContext = ViewModelsFactory.Topic();
            this.Loaded += TopicWindow_Loaded;
        }

        public TopicWindow(string topicId)
        {
            InitializeComponent();

            DataContext = ViewModelsFactory.UpdateTopic(topicId);
            this.Loaded += TopicWindow_Loaded;
        }

        private async void TopicWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Initialize();
        }
    }
}
