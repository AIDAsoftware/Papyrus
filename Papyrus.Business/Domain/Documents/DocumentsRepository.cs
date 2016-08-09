using Papyrus.Business.Domain.Products;

namespace Papyrus.Business.Domain.Documents {
    public interface DocumentsRepository {
        Documentation GetDocumentationFor(VersionIdentifier versionId);
        void CreateDocumentFor(Document document);
    }
}