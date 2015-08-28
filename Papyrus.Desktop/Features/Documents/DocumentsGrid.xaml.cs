using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Papyrus.Desktop.Features.Documents {
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
