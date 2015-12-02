using System.Collections.Generic;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class WebsiteCollection {
        private readonly Dictionary<string, WebSite> websites;

        public int Count {
            get { return websites.Count; }
        }

        public WebSite this[string path] {
            get { return websites[path]; }
        }

        public WebsiteCollection() {
            websites = new Dictionary<string, WebSite>();
        }

        public void Add(string generateMkdocsPath, WebSite website) {
            websites.Add(generateMkdocsPath, website);
        }
    }
}