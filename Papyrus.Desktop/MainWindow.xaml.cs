using System.Collections.ObjectModel;
using System.Windows;

using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.MainMenu;
using Papyrus.Desktop.Features.Topics;

namespace Papyrus.Desktop
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<DisplayableProduct> Products;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowVM()
            {
                MainMenuVM = ViewModelsFactory.MainMenu(),
                TopicsGridVM = ViewModelsFactory.TopicsGrid()
            };
        }

        public MainWindowVM ViewModel { get; set; }
    }

    public class MainWindowVM
    {
        public MainMenuVM MainMenuVM { get; set; }

        public TopicsGridVM TopicsGridVM { get; set; }
    }
}
