using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class Documents : IEnumerable
    {
        private List<Document> documents;

        public Documents()
        {
            documents = new List<Document>();
        }

        public void Add(Document document)
        {
            documents.Add(document);
        }

        public Document this[string language]
        {
            get { return documents.FirstOrDefault(d => d.Language == language); }
        }

        public IEnumerator<Document> GetEnumerator()
        {
            return documents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}