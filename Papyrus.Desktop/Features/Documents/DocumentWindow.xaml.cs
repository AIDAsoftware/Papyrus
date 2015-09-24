using System.Windows;
using System.Windows.Controls;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Documents {
    /// <summary>
    /// Interaction logic for DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window {
        
        public NewDocumentVM ViewModel
        {
            get { return (NewDocumentVM)DataContext; }
        }

        public DocumentWindow(DocumentDetails document)
        {
            InitializeComponent();
            DataContext = ViewModelsFactory.UpdateDocumentWindowVm(document);
            this.Loaded += DocumentsGrid_Loaded;
        }

        public DocumentWindow() {
            InitializeComponent();
            DataContext = ViewModelsFactory.NewDocumentWindowVm();
            this.Loaded += DocumentsGrid_Loaded;
        }

        private async void DocumentsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Initialize();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveDocument();
        }

        private void Product_OnSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var product = (Product)((ComboBox) sender).SelectedValue;
            ViewModel.Versions.Clear();
            product.Versions.ForEach(v => ViewModel.Versions.Add(v));
        }
    }
}
