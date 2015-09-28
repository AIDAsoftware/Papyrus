using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Documents;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Documents {
    public class DocumentsGridVM {
        private readonly DocumentService documentService;
        private readonly ProductRepository productRepository;
        public ObservableCollection<DocumentDetails> Documents { get; private set; }
        public DocumentDetails SelectedDocument { get; set; }

        public DocumentsGridVM() {
            Documents = new ObservableCollection<DocumentDetails>();
        }

        public DocumentsGridVM(DocumentService documentService, ProductRepository productRepository) : this() {
            this.documentService = documentService;
            this.productRepository = productRepository;
        }

        public async Task Initialize() {
            await LoadAllDocuments();
        }

        private async Task LoadAllDocuments() {
            var documents = await documentService.AllDocuments();
            Documents.Clear();
            foreach (var document in documents) {
                Documents.Add(new DocumentDetails {
                    TopicId = document.DocumentIdentity.TopicId,
                    Product = (await productRepository.GetProduct(document.DocumentIdentity.ProductId)),
                    Version = await productRepository.GetVersion(document.DocumentIdentity.VersionId),
                    Language = document.DocumentIdentity.Language,
                    Title = document.Title,
                    Description = document.Description,
                    Content = document.Content
                });
            }
        }

        public async Task<Document> GetDocumentByTopicId(string topicId)
        {
            return await documentService.GetDocumentByTopicId(topicId);
        }

        public async void RefreshDocuments() {
            await LoadAllDocuments();
        }

        public async void ExportDocumentsToFolder() {
            await documentService.ExportDocumentsToFolder(new DirectoryInfo(@"C:\exportDirectory\docs"));
        }
    }

    public class DocumentDetails
    {
        public string TopicId { get; set; }
        public Product Product { get; set; }
        public ProductVersion Version { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    public class DesignModeDocumentsGridVM : DocumentsGridVM {
        public DesignModeDocumentsGridVM()
        {
            var product1 = new Product("anyId", "Papyrus", new List<ProductVersion>());
            var product2 = new Product("anyId", "Opportunity", new List<ProductVersion>());
            var version1 = new ProductVersion("anyId", "1.0");
            var version2 = new ProductVersion("anyId", "2.0");
            Documents.Add(new DocumentDetails {Product = product1, Description = "Describe how to use Papyrus", Title = "First Step", Version = version1, Language = "en-EN"});
            Documents.Add(new DocumentDetails {Product = product2, Description = "Describe how to use Opportunity", Title = "How to call", Version = version2, Language = "en-EN"});
        }
    }

}
