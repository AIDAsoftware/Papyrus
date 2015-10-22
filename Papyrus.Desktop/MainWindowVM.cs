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
    }
}