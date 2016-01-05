using System;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public class VersionRange
    {
        public string FromVersionId { get; private set; }
        public string ToVersionId { get; private set; }
        public string VersionRangeId { get; private set; }

        public Documents Documents { get; private set; }

        public VersionRange(ProductVersion fromVersion, ProductVersion toVersion) {
            FromVersionId = fromVersion.VersionId;
            ToVersionId = toVersion.VersionId;
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

        public void GenerateRecursiveAutomaticId()
        {
            VersionRangeId = Guid.NewGuid().ToString();
            foreach (var document in Documents)
            {
                document.GenerateAutomaticId();
            }
        }
    }
}