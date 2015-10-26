using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;

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

        public VersionRangeVM(EditableVersionRange editableVersionRange, ProductRepository productRepository)
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
            this.productRepository = productRepository;
            VersionRange = editableVersionRange;
        }

        public async void Initialize()
        {
//            var productVersions = await productRepository.GetAllVersionsFor(SelectedProduct.ProductId);
//            productVersions.ForEach(pv => ProductVersions.Add(pv));
        }
    }
}