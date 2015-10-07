using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM
    {
        private readonly ProductRepository productRepository;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public TopicVM()
        {
            Languages = new ObservableCollection<string>();
            Products = new ObservableCollection<Product>();
        }

        public TopicVM(ProductRepository productRepository) : this()
        {
            this.productRepository = productRepository;
        }

        public async Task Initialize()
        {
            Languages.Add("es-ES");
            Languages.Add("en-GB");
            var allProductsAvailable = await productRepository.GetAllProducts();
            foreach (var product in allProductsAvailable)
            {
                Products.Add(product);
            }
        }                                       
    }
}