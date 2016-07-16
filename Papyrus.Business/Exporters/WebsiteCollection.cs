using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class WebsiteCollection : IEnumerable {
        private readonly List<List<WebSite>> websites;

        public int Count => websites.Count;

        public WebsiteCollection() {
            websites = new List<List<WebSite>>();
        }

        public void Add(WebSite website) {
            websites.Add(new List<WebSite> {website});
        }

        public IEnumerator<List<WebSite>> GetEnumerator() {
            return websites.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public List<WebSite> First()
        {
            return websites.First();
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