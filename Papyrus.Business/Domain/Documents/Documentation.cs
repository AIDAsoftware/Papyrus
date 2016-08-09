using System.Collections.Generic;

namespace Papyrus.Business.Domain.Documents {
    public class Documentation {
        private Documentation(List<Document> documents) {
            Documents = documents;
        }

        public static Documentation WithDocuments(List<Document> documents) {
            return new Documentation(documents);
        }

        //TODO private
        public List<Document> Documents { get; set; }

        //TODO: Readonlycollection
        public List<Document> ToList() {
            return Documents;
        }
    }
}