using System.Collections.Generic;

namespace Papyrus.Business.Topics
{
    public class VersionRange
    {
        public string FromVersionId { get; }
        public string ToVersionId { get; }
        public string VersionRangeId { get; private set; }

        public Dictionary<string, Document2> documents { get; }

        public VersionRange(string fromVersionId, string toVersionId)
        {
            FromVersionId = fromVersionId;
            ToVersionId = toVersionId;
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

        public VersionRange WithId(string id)
        {
            VersionRangeId = id;
            return this;
        }
    }
}