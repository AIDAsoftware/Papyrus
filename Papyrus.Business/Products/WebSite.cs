using System.Collections.Generic;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Products {
    public class WebSite {
        public List<ExportableDocument> documents { get; set; }

        public WebSite() {
            documents = new List<ExportableDocument>();
        }

        public void AddDocument(ExportableDocument document) {
            documents.Add(document);
        }
    }
}