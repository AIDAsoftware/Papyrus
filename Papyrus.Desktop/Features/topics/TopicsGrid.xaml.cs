using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Papyrus.Business.Documents;

namespace Papyrus.Desktop.Features.Topics {
    public partial class TopicsGrid : UserControl {
        public TopicsGridVm ViewModel {
            get { return (TopicsGridVm)DataContext; }
        }

        public DisplayableProduct SelectedProduct { get; set; }

        public TopicsGrid() {
            InitializeComponent();

            DataContext = ViewModelsFactory.TopicsGrid();
            Loaded += TopicGrid_Loaded;
        }

        private async void TopicGrid_Loaded(object sender, RoutedEventArgs e) {
            await ViewModel.Initialize();
        }

        private async void NewTopic_OnClick(object sender, RoutedEventArgs e) {
            if (ViewModel.SelectedProduct != null) {
                var topic = await ViewModel.PrepareNewDocument();
                new TopicWindow(topic).Show();
            }
        }

        private async void TopicRow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var topicId = ViewModel.SelectedTopic.TopicId;
            var topic = await ViewModel.GetEditableTopicById(topicId);
            new TopicWindow(topic).Show();
        }
    }
}
