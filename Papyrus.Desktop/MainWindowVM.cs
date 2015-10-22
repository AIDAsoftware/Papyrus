using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;

namespace Papyrus.Desktop
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private readonly ProductRepository productRepository;
        public DisplayableProduct SelectedProduct { get; set; }
        public ObservableCollection<DisplayableProduct> Products { get; set; }

        public MainWindowVM(ProductRepository productRepository)
        {
            Products = new ObservableCollection<DisplayableProduct>();
            this.productRepository = productRepository;
        }

        public async void Initialize()
        {
            var products = await productRepository.GetAllDisplayableProducts();
            products.ForEach(p => Products.Add(p));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<EditableTopic> PrepareNewDocument()
        {
            var fullVersionRange = await productRepository.GetFullVersionRangeForProduct(SelectedProduct.ProductId);
            var editableVersionRange = await DefaultVersionRange(fullVersionRange);
            var versionRanges = new ObservableCollection<EditableVersionRange> {editableVersionRange};
            return new EditableTopic
            {
                Product = SelectedProduct,
                VersionRanges = versionRanges
            };
        }

        private async Task<EditableVersionRange> DefaultVersionRange(FullVersionRange fullVersionRange)
        {
            var editableVersionRange = new EditableVersionRange
            {
                FromVersion = await productRepository.GetVersion(fullVersionRange.FirstVersionId),
                ToVersion = await productRepository.GetVersion(fullVersionRange.LatestVersionId)
            };
            editableVersionRange.Documents.Add(new EditableDocument {Language = "es-ES"});
            editableVersionRange.Documents.Add(new EditableDocument {Language = "en-GB"});
            return editableVersionRange;
        }
    }
}