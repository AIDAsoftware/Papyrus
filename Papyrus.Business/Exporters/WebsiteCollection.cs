using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class WebsiteCollection : IEnumerable {
        private readonly Dictionary<string, List<WebSite>> websites;

        public int Count {
            get { return websites.Count; }
        }

        public List<WebSite> this[string path] {
            get { return websites[path]; }
        }

        public WebsiteCollection() {
            websites = new Dictionary<string, List<WebSite>>();
        }

        public void Add(string path, WebSite website) {
            if (websites.ContainsKey(path))
                websites[path].Add(website);
            else 
                websites.Add(path, new List<WebSite>{website});
        }

        public IEnumerator<WebsitePathPair> GetEnumerator() {
            return websites.Select(website => new WebsitePathPair(website.Value, website.Key)).GetEnumerator();
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