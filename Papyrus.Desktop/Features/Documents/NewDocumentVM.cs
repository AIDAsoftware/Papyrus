using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Documents
{
    public class NewDocumentVM
    {
        private readonly ProductRepository productRepository;
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<string> Versions { get; private set; }
        public ObservableCollection<string> Languages { get; private set; }

        public NewDocumentVM()
        {
            Products = new ObservableCollection<Product>();
        }

        public NewDocumentVM(ProductRepository productRepository) : this()
        {
            this.productRepository = productRepository;
        }

        public async Task Initialize()
        {
            var products = (await productRepository.GetAllProducts()).ToList();
            products.ForEach(product => Products.Add(product));
            Versions = new ObservableCollection<string> {};
            Languages = new ObservableCollection<string> {"es-ES", "en-GB"};
        }
    }
}