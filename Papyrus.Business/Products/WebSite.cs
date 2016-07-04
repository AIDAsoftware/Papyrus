using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Products {
    public class WebSite {
        private readonly string productName;
        public string ProductName { get { return productName; } }
        public IList<ExportableDocument> Documents { get; set; }

        public WebSite(IList<ExportableDocument> documents, string productName) {
            this.productName = productName;
            Documents = documents;
        }

        public void AddDocument(ExportableDocument document) {
            Documents.Add(document);
        }

        public bool HasNotDocuments() {
            return !Documents.Any();
        }
    }
}