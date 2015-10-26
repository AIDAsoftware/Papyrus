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
        }
    }
}
