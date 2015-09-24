using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Desktop.Annotations;

namespace Papyrus.Desktop.Features.Documents
{
    public class NewDocumentVM : INotifyPropertyChanged
    {
        private readonly ProductRepository productRepository;
        private readonly DocumentService documentService;
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<ProductVersion> Versions { get; private set; }
        public ObservableCollection<string> Languages { get; private set; }
        public DocumentDetails Document { get; set; }

        public Product SelectedProduct
        {
            get { return Document.Product; }
            set
            {
                Document.Product = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public ProductVersion SelectedProductVersion
        {
            get { return Document.Version; }
            set
            {
                Document.Version = value;
                OnPropertyChanged("SelectedProductVersion");
            }
        }

        public string SelectedLanguage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public NewDocumentVM()
        {
            Products = new ObservableCollection<Product>();
            Versions = new ObservableCollection<ProductVersion>();
            Languages = new ObservableCollection<string>();
            Document = new DocumentDetails();
        }

        public NewDocumentVM(ProductRepository productRepository, DocumentService documentService, DocumentDetails document) : this(productRepository, documentService)
        {
            Document = document;
        }

        public NewDocumentVM(ProductRepository productRepository, DocumentService documentService) : this()
        {
            this.productRepository = productRepository;
            this.documentService = documentService;
        }

        public async Task Initialize()
        {
            var products = (await productRepository.GetAllProducts()).ToList();
            products.ForEach(product => Products.Add(product));
            if (Document.TopicId != null)
            {
                SelectedProduct = Products.First((p) => p.Id == Document.Product.Id);
                SelectedProductVersion = (await productRepository.GetVersion(Document.Version.VersionId));
            }
            AddPossibleLanguages();
        }

        public async void SaveDocument()
        {
            var document = DocumentFromForm();

            if (string.IsNullOrEmpty(Document.TopicId)) {
                await documentService.Create(document);
            }

            document.WithTopicId(Document.TopicId);
            await documentService.Update(document);
        }


        private void AddPossibleLanguages()
        {
            Languages.Add("es-ES");
            Languages.Add("en-GB");
        }

        private Document DocumentFromForm()
        {
            return new Document()
                .ForProduct(Document.Product.Id)
                .ForProductVersion(Document.Version.VersionId)
                .ForLanguage(Document.Language)
                .WithTitle(Document.Title)
                .WithDescription(Document.Description)
                .WithContent(Document.Content);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}