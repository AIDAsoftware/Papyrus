using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Papyrus.Desktop.Features.Topics
{
    /// <summary>
    /// Interaction logic for TopicWindow.xaml
    /// </summary>
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

        private async void Save_OnClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveTopic();
        }
    }
}
