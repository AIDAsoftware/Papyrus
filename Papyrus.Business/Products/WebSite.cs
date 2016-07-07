using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Products {
    public class WebSite {
        private readonly string productName;
        private readonly string language;
        private readonly string version;
        public string ProductName { get { return productName; } }
        public IList<ExportableDocument> Documents { get; set; }
        public string Language { get { return language; } }

        public string Version { get { return version; } }
        public WebSite(IList<ExportableDocument> documents, string productName, string language = "", string version = "") {
            this.productName = productName;
            this.language = language;
            Documents = documents;
            this.version = version;
        }

        public void AddDocument(ExportableDocument document) {
            Documents.Add(document);
        }

        public bool HasNotDocuments() {
            return !Documents.Any();
        }
    }
}