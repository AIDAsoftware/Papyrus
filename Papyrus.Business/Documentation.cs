using System.Collections.Generic;

namespace Papyrus.Business {
    public class Documentation {
        public void AddDocuments(List<Document> documents) {
            Documents = documents;
        }

        public List<Document> Documents { get; set; }


        public List<Document> ToList() {
            return Documents;
        }
    }
}