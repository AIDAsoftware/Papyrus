using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Papyrus.Desktop.Documents {
    /// <summary>
    /// Interaction logic for DocumentsGrid.xaml
    /// </summary>
    public partial class DocumentsGrid : UserControl {
        public DocumentsGridVM ViewModel {
            get { return (DocumentsGridVM)DataContext; }
        }

        public DocumentsGrid() {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
                DataContext = new DesignModeDocumentsGridVM();
            } else {
                DataContext = ViewModelsFactory.DocumentsGrid();
                this.Loaded += DocumentsGrid_Loaded;
            }
        }

        private async void DocumentsGrid_Loaded(object sender, RoutedEventArgs e) {
            await ViewModel.Initialize();
        }
    }
}
