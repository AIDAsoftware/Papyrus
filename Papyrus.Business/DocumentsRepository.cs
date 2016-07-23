namespace Papyrus.Business {
    public interface DocumentsRepository {
        Documentation GetDocumentationFor(string productId, string versionId);
    }
}