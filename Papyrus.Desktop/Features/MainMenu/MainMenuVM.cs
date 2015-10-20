using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.MainMenu {
    public class MainMenuVM {
        private readonly ProductRepository productRepository;
        public ObservableCollection<DisplayableProduct> Products { get; private set; }

        protected MainMenuVM() {
            Products = new ObservableCollection<DisplayableProduct>();
        }

        public MainMenuVM(ProductRepository productRepository) : this() {
            this.productRepository = productRepository;
        }

        public async Task Initialize()
        {
            var products = await productRepository.GetAllDisplayableProducts();
            products.ForEach(Products.Add);
        }
    }

    public class DesignModeMainMenuVM : MainMenuVM {
        public DesignModeMainMenuVM() {
            Products.Add(new DisplayableProduct { ProductId = "OpportunityID", ProductName = "Opportunity"});
            Products.Add(new DisplayableProduct { ProductId = "PapyrusID", ProductName = "Papyrus"});
        }
    }

}
