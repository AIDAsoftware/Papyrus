using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Documents
{
    public class NewDocumentVM
    {
        private readonly ProductRepository productRepository;
        private readonly DocumentService documentService;
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<ProductVersion> Versions { get; private set; }
        public ObservableCollection<string> Languages { get; private set; }
        public DocumentDetails Document { get; set; }
        public Product SelectedProduct { get; set; }
        public ProductVersion SelectedProductVersion { get; set; }
        public string SelectedLanguage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public NewDocumentVM()
        {
            Products = new ObservableCollection<Product>();
            Versions = new ObservableCollection<ProductVersion>();
            Languages = new ObservableCollection<string>();
        }

        public NewDocumentVM(ProductRepository productRepository, DocumentService documentService, DocumentDetails document) : this(productRepository, documentService)
        {
            Document = document;
            Console.WriteLine(document.Title);
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
            AddPossibleLanguages();
        }

        private void AddPossibleLanguages()
        {
            Languages.Add("es-ES");
            Languages.Add("en-GB");
        }

        public void SaveDocument()
        {
            var document = new Document()
                .ForProduct(SelectedProduct.Id)
                .ForProductVersion(SelectedProductVersion.VersionId)
                .ForLanguage(SelectedLanguage)
                .WithTitle(Title)
                .WithDescription(Description)
                .WithContent(Content);
            documentService.Create(document);
        }
    }
}