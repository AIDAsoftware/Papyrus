using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Papyrus.Desktop.Features.Topics
{
    /// <summary>
    /// Interaction logic for TopicsGrid.xaml
    /// </summary>
    public partial class TopicsGrid : UserControl
    {
        public TopicsGridVM ViewModel
        {
            get { return (TopicsGridVM)DataContext; }
        }

        public TopicsGrid()
        {
            InitializeComponent();

            DataContext = ViewModelsFactory.TopicsGrid();
            this.Loaded += TopicGrid_Loaded;
        }

        private async void TopicGrid_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Initialize();
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RefreshDocuments();
        }

        private void ExportToFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TopicRow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var topicId = ViewModel.SelectedTopic.TopicId;
            new TopicWindow(topicId).Show();
        }
    }
}
