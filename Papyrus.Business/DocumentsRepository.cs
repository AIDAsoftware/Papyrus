namespace Papyrus.Business {
    public interface DocumentsRepository {
        Documentation GetDocumentationFor(VersionIdentifier versionId);
        void CreateDocumentFor(Document document);
    }
}