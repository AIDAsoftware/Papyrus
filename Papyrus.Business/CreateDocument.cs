namespace Papyrus.Business {
    public class CreateDocument {
        private DocumentsRepository DocumentsRepository { get; }

        public CreateDocument(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public void ExecuteFor(DocumentDto documentDto) {
            DocumentsRepository.CreateDocumentFor(ToDocument(documentDto));
        }

        private static Document ToDocument(DocumentDto document) {
            return Document.CreateDocument(document.Title, 
                document.Description, 
                document.Content, 
                document.Language,
                document.ProductId,
                document.VersionId);
        }
    }
}