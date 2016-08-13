using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Business.Actions {
    public class CreateDocument {
        private DocumentsRepository DocumentsRepository { get; }

        public CreateDocument(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public void ExecuteFor(DocumentDto documentDto) {
            DocumentsRepository.CreateDocumentFor(ToDocument(documentDto));
        }

        private static Document ToDocument(DocumentDto document) {
            return new Document(
                new DocumentId(document.Id), 
                document.Title, 
                document.Description, 
                document.Content, 
                document.Language, 
                new VersionIdentifier(document.ProductId, document.VersionId));
        }
    }
}