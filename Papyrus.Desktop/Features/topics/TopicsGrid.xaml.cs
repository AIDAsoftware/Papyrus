using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class TopicsGrid : UserControl
    {
        public TopicsGridVM ViewModel
        {
            get { return (TopicsGridVM)DataContext; }
        }

        public static readonly DependencyProperty SelectedProductProperty =
                DependencyProperty.Register("SelectedProduct", typeof(DisplayableProduct), typeof(TopicsGrid));
        public DisplayableProduct SelectedProduct
        {
            get
            {
                return this.GetValue(SelectedProductProperty) as DisplayableProduct;
            }
            set
            {
                this.SetValue(SelectedProductProperty, value);
            }
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

        private void ExportToFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void TopicRow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var topicId = ViewModel.SelectedTopic.TopicId;
            var topic = await ViewModel.GetEditableTopicById(topicId);
            new TopicWindow(topic).Show();
        }
    }
}
