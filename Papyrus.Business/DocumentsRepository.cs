namespace Papyrus.Business {
    public interface DocumentsRepository {
        Documentation GetDocumentationFor(string productId, string versionId);
        void CreateDocumentFor(Document document, string productId, string versionId);
    }
}