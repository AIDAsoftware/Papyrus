using System.Collections.ObjectModel;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeVM
    {
        private readonly ProductRepository productRepository;
        public EditableVersionRange VersionRange { get; set; }
        public ObservableCollection<ProductVersion> ProductVersions { get; private set; }
        public DisplayableProduct SelectedProduct { get; private set; }

        public VersionRangeVM()
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
        }

        public VersionRangeVM(EditableVersionRange editableVersionRange, DisplayableProduct selectedProduct, ProductRepository productRepository)
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
            this.productRepository = productRepository;
            VersionRange = editableVersionRange;
            SelectedProduct = selectedProduct;
        }

        public async void Initialize()
        {
            var productVersions = await productRepository.GetAllVersionsFor(SelectedProduct.ProductId);
        }
    }
}