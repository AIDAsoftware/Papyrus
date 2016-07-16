using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class WebsiteCollection : IEnumerable<WebSite> {
        private readonly List<WebSite> websites;

        public WebsiteCollection() {
            websites = new List<WebSite>();
        }

        public void Add(WebSite website) {
            websites.Add(website);
        }

        public IEnumerator<WebSite> GetEnumerator() {
            return websites.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class WebsitePathPair {
        public List<WebSite> Websites { get; private set; }
        public string Path { get; private set; }

        public WebsitePathPair(List<WebSite> websites, string path) {
            Websites = websites;
            Path = path;
        }
    }
}