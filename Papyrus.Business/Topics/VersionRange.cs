using System;

namespace Papyrus.Business.Topics
{
    public class VersionRange
    {
        public string FromVersionId { get; }
        public string ToVersionId { get; }
        public string VersionRangeId { get; private set; }

        public Documents Documents { get; }

        public VersionRange(string fromVersionId, string toVersionId)
        {
            FromVersionId = fromVersionId;
            ToVersionId = toVersionId;
            Documents = new Documents();
        }

        public void AddDocument(string language, Document2 document)
        {
            Documents.Add(document);
        }

        public void AddDocument(Document2 document)
        {
            Documents.Add(document);
        }

        public Document2 GetDocumentIn(string language)
        {
            return Documents[language];
        }

        public VersionRange WithId(string id)
        {
            VersionRangeId = id;
            return this;
        }

        public void GenerateAutomaticId()
        {
            VersionRangeId = Guid.NewGuid().ToString();
            foreach (var document2 in Documents)
            {
                document2.GenerateAutomaticId();
            }
        }
    }
}