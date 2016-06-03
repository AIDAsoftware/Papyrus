using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Products {
    public class WebSite {
        public IList<ExportableDocument> Documents { get; set; }

        public WebSite(IList<ExportableDocument> documents) {
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