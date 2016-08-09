using System.Collections.Generic;

namespace Papyrus.Business.Domain.Documents {
    public class Documentation {
        private List<Document> Documents { get; }

        private Documentation(List<Document> documents) {
            Documents = documents;
        }

        public static Documentation WithDocuments(List<Document> documents) {
            return new Documentation(documents);
        }

        public IReadOnlyCollection<Document> AsDocumentsCollection() {
            return Documents;
        }
    }
}