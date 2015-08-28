using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Documents;

namespace Papyrus.Desktop.Features.Documents {
    public class DocumentsGridVM {
        private readonly DocumentService documentService;
        public ObservableCollection<Document> Documents { get; private set; }

        public DocumentsGridVM() {
            Documents = new ObservableCollection<Document>();
        }

        public DocumentsGridVM(DocumentService documentService) : this() {
            this.documentService = documentService;
        }

        public async Task Initialize() {
            var documents = await documentService.AllDocuments();
            Documents.Clear();
            foreach (var document in documents) {
                Documents.Add(document);
            }
        }
    }

    public class DesignModeDocumentsGridVM : DocumentsGridVM {
        public DesignModeDocumentsGridVM() {
            Documents.Add(new Document().WithTitle("Title 1").ForLanguage("es-ES").WithDescription("Description 1"));
            Documents.Add(new Document().WithTitle("Title 2").ForLanguage("es-ES").WithDescription("Description 2"));
        }
    }

}
