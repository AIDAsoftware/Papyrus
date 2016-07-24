using System.Collections.Generic;

namespace Papyrus.Business {
    public class Documentation {
        private Documentation(List<Document> documents) {
            Documents = documents;
        }

        public static Documentation WithDocuments(List<Document> documents) {
            return new Documentation(documents);
        }

        public List<Document> Documents { get; set; }


        public List<Document> ToList() {
            return Documents;
        }
    }
}