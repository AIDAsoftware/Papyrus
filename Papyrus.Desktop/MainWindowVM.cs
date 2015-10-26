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
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task<EditableTopic> PrepareNewDocument()
        {
            var fullVersionRange = await DefaultVersionRange();
            var versionRanges = new ObservableCollection<EditableVersionRange> {fullVersionRange};
            return new EditableTopic
            {
                Product = SelectedProduct,
                VersionRanges = versionRanges
            };
        }

        private async Task<EditableVersionRange> DefaultVersionRange()
        {
            var fullVersionRange = await productRepository.GetFullVersionRangeForProduct(SelectedProduct.ProductId);
            var editableVersionRange = new EditableVersionRange
            {
                FromVersion = await productRepository.GetVersion(fullVersionRange.FirstVersionId),
                ToVersion = await productRepository.GetVersion(fullVersionRange.LatestVersionId)
            };
            AddDefaultLanguages(editableVersionRange);
            return editableVersionRange;
        }

        private static void AddDefaultLanguages(EditableVersionRange editableVersionRange)
        {
            editableVersionRange.Documents.Add(new EditableDocument {Language = "es-ES"});
            editableVersionRange.Documents.Add(new EditableDocument {Language = "en-GB"});
        }
    }

    public class DesignModeMainWindowVM : MainWindowVM
    {
        public DesignModeMainWindowVM(ProductRepository productRepository) : base(productRepository)
        {
            Products = new ObservableCollection<DisplayableProduct>
            {
                new DisplayableProduct {ProductId = "OpportunityId", ProductName = "Opportunity"}
            };
            SelectedProduct = Products[0];
        }
    }
}