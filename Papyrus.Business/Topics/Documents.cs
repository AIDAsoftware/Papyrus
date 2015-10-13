using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class Documents : IEnumerable
    {
        private Dictionary<string, Document2> documents;

        public Documents()
        {
            documents = new Dictionary<string, Document2>();
        }

        public void Add(string language, Document2 document)
        {
            documents.Add(language, document);
        }

        public Document2 this[string language]
        {
            get { return documents[language]; }
        }

        public IEnumerator<LanguageDocumentPair> GetEnumerator()
        {
            return documents.Select(document => new LanguageDocumentPair(document.Key, document.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}