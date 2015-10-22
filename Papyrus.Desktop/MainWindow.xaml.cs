using System.Collections.ObjectModel;
using System.Windows;

namespace Papyrus.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindowVM ViewModel
        {
            get { return (MainWindowVM) DataContext; }
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModelsFactory.MainWindow();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }

        private void NewTopic_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
