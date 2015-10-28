using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;
using Papyrus.Desktop.Util.Command;

namespace Papyrus.Desktop.Features.Topics {
    public class TopicsGridVM : INotifyPropertyChanged
    {
        private readonly TopicRepository topicRepository;
        private readonly ProductRepository productRepository;
        public ObservableCollection<TopicSummary> TopicsToList { get; protected set; }
        public ObservableCollection<DisplayableProduct> Products { get; private set; }
        public TopicSummary SelectedTopic { get; set; }

        private DisplayableProduct selectedProduct;
        public DisplayableProduct SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
                RefreshTopicsForCurrentProduct();
            }
        }

        public IAsyncCommand RefreshTopics { get; set; }

        protected TopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>();
            Products = new ObservableCollection<DisplayableProduct>();
            RefreshTopics = RelayAsyncSimpleCommand.Create(LoadAllTopics, CanLoadAllTopics);
        }

        public TopicsGridVM(TopicRepository topicRepository, ProductRepository productRepository) : this()
        {
            this.topicRepository = topicRepository;
            this.productRepository = productRepository;
        }

        public async Task Initialize()
        {
            await LoadAllProducts();
            SelectedProduct = Products[0];
            await LoadAllTopics();
        }

        public async void RefreshTopicsForCurrentProduct()
        {
            await LoadAllTopics();
        }

        public async Task<EditableTopic> GetEditableTopicById(string topicId)
        {
            return await topicRepository.GetEditableTopicById(topicId);
        }

        public async Task<EditableTopic> PrepareNewDocument()
        {
            var fullVersionRange = await DefaultVersionRange();
            var versionRanges = new ObservableCollection<EditableVersionRange> { fullVersionRange };
            return new EditableTopic
            {
                Product = SelectedProduct,
                VersionRanges = versionRanges
            };
        }


        private async Task LoadAllProducts()
        {
            var products = await productRepository.GetAllDisplayableProducts();
            products.ForEach(p => Products.Add(p));
        }

        private async Task LoadAllTopics()
        {
            canLoadTopics = false;
            TopicsToList.Clear();
            (await topicRepository.GetAllTopicsSummaries())
                .Where(t => t.Product.ProductId == SelectedProduct.ProductId)
                .ToList()
                .ForEach(topic => TopicsToList.Add(topic));
            canLoadTopics = true;
        }

        private bool canLoadTopics;
        private bool CanLoadAllTopics()
        {
            return canLoadTopics;
        }

        private async Task<EditableVersionRange> DefaultVersionRange()
        {
            var fullVersionRange = await productRepository.GetFullVersionRangeForProduct(SelectedProduct.ProductId);
            var editableVersionRange = new EditableVersionRange
            {
                FromVersion = await productRepository.GetVersion(fullVersionRange.FirstVersionId),
                ToVersion = await productRepository.GetVersion(fullVersionRange.LatestVersionId)
            };
            AddDocumentsForDefaultLanguages(editableVersionRange);
            return editableVersionRange;
        }

        private static void AddDocumentsForDefaultLanguages(EditableVersionRange editableVersionRange)
        {
            editableVersionRange.Documents.Add(new EditableDocument { Language = "es-ES" });
            editableVersionRange.Documents.Add(new EditableDocument { Language = "en-GB" });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DesignModeTopicsGridVM : TopicsGridVM
    {
        public DesignModeTopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>
            {
                new TopicSummary
                {
                    LastDocumentTitle = "Login",
                    LastDocumentDescription = "Explicación",
                    VersionName = "2.0",
                    Product = new DisplayableProduct {ProductId = "ProductId", ProductName = "Opportunity"}
                },
                new TopicSummary
                {
                    LastDocumentTitle = "Llamadas",
                    LastDocumentDescription = "Explicación",
                    VersionName = "3.0",
                    Product = new DisplayableProduct {ProductId = "ProductId", ProductName = "Opportunity"}
                }
            };
        }
    }
}
