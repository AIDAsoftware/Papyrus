using System.Collections.Generic;

namespace Papyrus.Business {
    public class GetDocumentation {
        public DocumentsRepository DocumentsRepository { get; set; }

        public GetDocumentation(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public List<Document> ExecuteFor(string productId, string versionId) {
            return DocumentsRepository.GetDocumentationFor(new VersionIdentifier(productId, versionId)).ToList();
        }
    }
}