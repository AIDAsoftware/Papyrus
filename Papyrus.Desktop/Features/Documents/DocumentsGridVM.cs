using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;

namespace Papyrus.Desktop.Features.Documents {
    public class DocumentsGridVM {
        private readonly DocumentService documentService;
        private readonly ProductRepository productRepository;
        public ObservableCollection<DocumentView> Documents { get; private set; }

        public DocumentsGridVM() {
            Documents = new ObservableCollection<DocumentView>();
        }

        public DocumentsGridVM(DocumentService documentService, ProductRepository productRepository) : this() {
            this.documentService = documentService;
            this.productRepository = productRepository;
        }

        public async Task Initialize() {
            var documents = await documentService.AllDocuments();
            Documents.Clear();
            foreach (var document in documents) {
                Documents.Add(new DocumentView
                {
                    Product = (await productRepository.GetProduct(document.DocumentIdentity.ProductId)).Name,
                    Version = await productRepository.GetVersion(document.DocumentIdentity.VersionId),
                    Language = document.DocumentIdentity.Language,
                    Title = document.Title,
                    Description = document.Description
                });
            }
        }
    }

    public class DocumentView
    {
        public string Product { get; set; }
        public string Version { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class DesignModeDocumentsGridVM : DocumentsGridVM {
        public DesignModeDocumentsGridVM()
        {
            Documents.Add(new DocumentView {Product = "Papyrus", Description = "Describe how to use Papyrus", Title = "First Step", Version = "2.0", Language = "en-EN"});
            Documents.Add(new DocumentView {Product = "Opportunity", Description = "Describe how to use Opportunity", Title = "How to call", Version = "1.0", Language = "en-EN"});
        }
    }

}
