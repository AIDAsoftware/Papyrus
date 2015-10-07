using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM
    {
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

        public async Task Initialize()
        {
            Languages.Add("es-ES");
            Languages.Add("en-GB");
            var productVersions = new List<ProductVersion>
            {
                new ProductVersion("1.0", "1.0")    
            };
            Products.Add(new Product("AnyId", "Opportunity", productVersions));
            Products.Add(new Product("AnotherId", "Papyrus", productVersions));
            Products.Add(new Product("AnotherOneId", "SIMA", productVersions));
        }                                       
    }
}