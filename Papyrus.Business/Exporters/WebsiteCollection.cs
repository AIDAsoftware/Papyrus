using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class WebsiteCollection : IEnumerable {
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

        public IEnumerator<WebsitePathPair> GetEnumerator() {
            return websites.Select(website => new WebsitePathPair(website.Value, website.Key)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class WebsitePathPair {
        public WebSite Website { get; private set; }
        public string Path { get; private set; }

        public WebsitePathPair(WebSite website, string path) {
            Website = website;
            Path = path;
        }
    }
}