using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;

namespace Papyrus.Desktop.Features.Topics
{
    public class TopicVM : INotifyPropertyChanged
    {
        private readonly ProductRepository productRepository;
        private readonly TopicService topicService;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        private string selectedLanguage;
        public string SelectedLanguage {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
            }
        }

        public TopicVM()
        {
            Languages = new ObservableCollection<string>();
            Products = new ObservableCollection<Product>();
        }

        public TopicVM(ProductRepository productRepository, TopicService topicService) : this()
        {
            this.productRepository = productRepository;
            this.topicService = topicService;
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

        public async Task SaveTopic()
        {
            var topic = new Topic(SelectedProduct.Id);
            var VersionIds = await productRepository.GetFullVersionRange();
            var fullVersionRange = new VersionRange(VersionIds.FirstVersionId, VersionIds.LatestVersionId);
            fullVersionRange.AddDocument(SelectedLanguage, new Document2(Title, Description, Content));
            topic.AddVersionRange(fullVersionRange);
            await topicService.Create(topic);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}