using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class Documents : IEnumerable
    {
        private List<Document2> documents;

        public Documents()
        {
            documents = new List<Document2>();
        }

        public void Add(Document2 document)
        {
            documents.Add(document);
        }

        public Document2 this[string language]
        {
            get { return documents.FirstOrDefault(d => d.Language == language); }
        }

        public IEnumerator<Document2> GetEnumerator()
        {
            return documents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}