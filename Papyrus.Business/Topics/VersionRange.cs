using System;

namespace Papyrus.Business.Topics
{
    public class VersionRange
    {
        public string FromVersionId { get; private set; }
        public string ToVersionId { get; private set; }
        public string VersionRangeId { get; private set; }

        public Documents Documents { get; private set; }

        public VersionRange(string fromVersionId, string toVersionId)
        {
            FromVersionId = fromVersionId;
            ToVersionId = toVersionId;
            Documents = new Documents();
        }

        public void AddDocument(Document document)
        {
            Documents.Add(document);
        }

        public Document GetDocumentIn(string language)
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