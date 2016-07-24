namespace Papyrus.Business {
    public class CreateDocument {
        private DocumentsRepository DocumentsRepository { get; }

        public CreateDocument(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public void ExecuteFor(DocumentDto documentDto, string productId, string versionId) {
            DocumentsRepository.CreateDocumentFor(ToDocument(documentDto), productId, versionId);
        }

        private static Document ToDocument(DocumentDto document) {
            return new Document(document.Title, document.Description, document.Content, document.Language);
        }
    }
}