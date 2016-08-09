using System.Collections.Generic;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Business.Actions {
    public class GetDocumentation {
        public DocumentsRepository DocumentsRepository { get; set; }

        public GetDocumentation(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public IReadOnlyCollection<Document> ExecuteFor(string productId, string versionId) {
            return DocumentsRepository
                .GetDocumentationFor(new VersionIdentifier(productId, versionId)).AsDocumentsCollection();
        }
    }
}