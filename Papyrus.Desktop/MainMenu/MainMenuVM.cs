using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.MainMenu {
    public class MainMenuVM {
        private ProductService productService;
        public ObservableCollection<Product> Products { get; protected set; }

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

    public class DesignModeMainMenuVm : MainMenuVM {
        public DesignModeMainMenuVm() {
            Products.Add(new Product("Any product name"));
            Products.Add(new Product("Any other product name"));
        }
    }

}
