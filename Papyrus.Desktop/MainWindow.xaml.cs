using System.Collections.ObjectModel;
using System.Windows;
using Papyrus.Desktop.Features.Topics;

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
