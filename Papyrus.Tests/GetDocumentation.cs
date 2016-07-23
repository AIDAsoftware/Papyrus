using System.Collections.Generic;
using Papyrus.Business;

namespace Papyrus.Tests {
    public class GetDocumentation {
        public DocumentsRepository DocumentsRepository { get; set; }

        public GetDocumentation(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public List<Document> ExecuteFor(string productId, string versionId) {
            return DocumentsRepository.GetDocumentationFor(productId, versionId).ToList();
        }
    }
}