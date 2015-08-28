using Papyrus.Business.Documents;

namespace Papyrus.Desktop.Features.Documents.Events {
    public class CreateNewDocument {
        public CreateNewDocument(Document document) {
            Document = document;
        }

        public Document Document { get; private set; }
    }
}