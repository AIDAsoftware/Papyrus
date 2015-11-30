using System.Collections.Generic;
using Papyrus.Business.Exporters;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Products {
    public class WebSite {
        public IList<ExportableDocument> Documents { get; set; }

        public WebSite(IList<ExportableDocument> documents) {
            Documents = documents;
        }

        public void AddDocument(ExportableDocument document) {
            Documents.Add(document);
        }
    }
}