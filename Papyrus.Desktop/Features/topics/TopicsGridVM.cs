using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Desktop.Annotations;
using Papyrus.Desktop.Util.Command;
using Papyrus.Infrastructure.Core.DomainEvents;


namespace Papyrus.Desktop.Features.Topics {
    public class TopicsGridVM : INotifyPropertyChanged
    {
        private readonly MkdocsExporter exporter;
        private readonly WebsiteConstructor websiteConstructor;
        private readonly TopicQueryRepository topicRepository;
        private readonly ProductRepository productRepository;
        public ObservableCollection<TopicSummary> TopicsToList { get; protected set; }
        public ObservableCollection<DisplayableProduct> Products { get; private set; }
        public TopicSummary SelectedTopic { get; set; }
        private string DefaultDirectoryPath { get; set; }

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

        public IAsyncCommand RefreshTopics { get; private set; }
        public IAsyncCommand ExportProductToMkDocs { get; private set; }
        public IAsyncCommand ExportLastVersionToMkDocs { get; private set; }
        public IAsyncCommand ExportAllProducts { get; private set; }


        protected TopicsGridVM()
        {
            TopicsToList = new ObservableCollection<TopicSummary>();
            Products = new ObservableCollection<DisplayableProduct>();
            RefreshTopics = RelayAsyncSimpleCommand.Create(LoadAllTopics, CanLoadAllTopics);
            ExportProductToMkDocs = RelayAsyncSimpleCommand.Create(ExportProduct, () => true);
            ExportLastVersionToMkDocs = RelayAsyncSimpleCommand.Create(ExportLastVersion, () => true);
            ExportAllProducts = RelayAsyncSimpleCommand.Create(ExportAllProductsDocumentation, () => true);
            DefaultDirectoryPath = Directory.GetCurrentDirectory();
            EventBus.AsObservable<OnTopicRemoved>().Subscribe(Handle);
        }

        //TODO: should it return a task?
        private async void Handle(OnTopicRemoved domainEvent) {
            await LoadAllTopics();
        }

        private async Task ExportLastVersion() {
            var product = CastToProductType(SelectedProduct);
            var versionName = (await productRepository.GetLastVersionForProduct(product.Id)).VersionName;
            var websiteCollection = await websiteConstructor.Construct(
                new PathByProductGenerator(), new List<Product> { product }, new List<string>{versionName}, languages
            );
            await Export(websiteCollection);
        }

        public TopicsGridVM(TopicQueryRepository topicRepo, ProductRepository productRepo, MkdocsExporter exporter, WebsiteConstructor websiteConstructor)
            : this(topicRepo, productRepo) {
            this.exporter = exporter;
            this.websiteConstructor = websiteConstructor;
        }

        private async Task ExportAllProductsDocumentation() {
            var products = MapDisplayableProductsToProducts(Products);
            var allVersionNames = await productRepository.GetAllVersionNames();
            var websiteCollection = await websiteConstructor.Construct(
                new PathByVersionGenerator(), products, allVersionNames, languages
            );
            await Export(websiteCollection);
        }

        private async Task ExportProduct() {
            var product = CastToProductType(SelectedProduct);
            var versionsNames = (await productRepository.GetAllVersionsFor(product.Id)).Select(v => v.VersionName).ToList();
            var websiteCollection = await websiteConstructor.Construct(
                new PathByProductGenerator(), new List<Product> { product }, versionsNames, languages
            );
            await Export(websiteCollection);
        }

        private async Task Export(WebsiteCollection websiteCollection) {
            foreach (var element in websiteCollection) {
                var fullPath = Path.Combine(DefaultDirectoryPath, element.Path);
                foreach (var website in element.Websites) {
                    await exporter.Export(website, fullPath);
                }
            }
        }

        private IEnumerable<Product> MapDisplayableProductsToProducts(ObservableCollection<DisplayableProduct> products) {
            return products.Select(p => CastToProductType(p));
        }

        private static Product CastToProductType(DisplayableProduct p) {
            return new Product(p.ProductId, p.ProductName, new List<ProductVersion>());
        }

        public TopicsGridVM(TopicQueryRepository topicRepository, ProductRepository productRepository) : this()
        {
            this.topicRepository = topicRepository;
            this.productRepository = productRepository;
        }

        public async Task Initialize()
        {
            await LoadAllProducts();
            SelectedProduct = Products.FirstOrDefault();
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
            (await topicRepository.GetAllTopicsSummariesFor("es-ES"))
                .Where(t => t.Product.ProductId == SelectedProduct.ProductId)
                .ToList()
                .ForEach(topic => TopicsToList.Add(topic));
            canLoadTopics = true;
        }

        private bool canLoadTopics;
        private readonly List<string> languages = new List<string>{ "es-ES", "en-GB" };

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

    public class OnTopicRemoved {}

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
