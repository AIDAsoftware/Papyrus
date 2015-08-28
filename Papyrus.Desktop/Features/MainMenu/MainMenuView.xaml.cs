using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Papyrus.Desktop.Features.Documents;

namespace Papyrus.Desktop.Features.MainMenu {
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : UserControl {

        public MainMenuVM ViewModel {
            get { return (MainMenuVM) DataContext; }
        }

        public MainMenuView() {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
                DataContext = new DesignModeMainMenuVM();
            }
            else {
                DataContext = ViewModelsFactory.MainMenu();
                this.Loaded += MainMenuView_Loaded;
            }
        }

        private async void MainMenuView_Loaded(object sender, RoutedEventArgs e) {
            await ViewModel.Initialize();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            new NewDocumentWindow().Show();
        }
    }
}
