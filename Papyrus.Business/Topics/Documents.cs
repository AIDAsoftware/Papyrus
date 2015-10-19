using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class Documents : IEnumerable
    {
        private Dictionary<string, Document2> documents;
        private List<Document2> documents2;

        public Documents()
        {
            documents2 = new List<Document2>();
        }

        public void Add(Document2 document)
        {
            documents2.Add(document);
        }

        public Document2 this[string language]
        {
            get { return documents2.FirstOrDefault(d => d.Language == language); }
        }

        public IEnumerator<Document2> GetEnumerator()
        {
            return documents2.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}