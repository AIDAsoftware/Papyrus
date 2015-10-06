using System.Collections.Generic;

namespace Papyrus.Business.Topics
{
    public class VersionRange
    {
        private Dictionary<string, Document2> documents;

        public VersionRange(string fromVersion, string toVersion)
        {
            documents = new Dictionary<string, Document2>();
        }

        public void AddDocument(string language, Document2 document)
        {
            documents.Add(language, document);
        }

        public Document2 GetDocumentIn(string language)
        {
            return documents[language];
        }
    }
}