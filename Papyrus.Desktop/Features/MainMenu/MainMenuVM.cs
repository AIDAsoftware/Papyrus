using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.MainMenu {
    public class MainMenuVM {
        private readonly ProductService productService;
        public ObservableCollection<Product> Products { get; private set; }

        protected MainMenuVM() {
            Products = new ObservableCollection<Product>();
        }

        public MainMenuVM(ProductService productService) : this() {
            this.productService = productService;
        }

        public async Task Initialize() {
            var products = await productService.AllProducts();
            Products.Clear(); 
            foreach (var product in products) {
                Products.Add(product);
            }
        }
    }

    public class DesignModeMainMenuVM : MainMenuVM {
        public DesignModeMainMenuVM() {
            Products.Add(new Product("Any product name"));
            Products.Add(new Product("Any other product name"));
        }
    }

}
