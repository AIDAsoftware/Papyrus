using System;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.Topics.Exceptions;
using Papyrus.Business.VersionRanges.Exceptions;

namespace Papyrus.Business.VersionRanges
{
    public class VersionRange
    {
        public ProductVersion FromVersion { get; private set; }
        public ProductVersion ToVersion { get; private set; }
        public string FromVersionId => FromVersion.VersionId;
        public string ToVersionId => ToVersion.VersionId;
        public string VersionRangeId { get; private set; }

        public Documents.Documents Documents { get; }

        public VersionRange(ProductVersion fromVersion, ProductVersion toVersion) {
            if (fromVersion.Release > toVersion.Release) throw new VersionRangeCannotBeDescendentException();
            FromVersion = fromVersion;
            ToVersion = toVersion;
            Documents = new Documents.Documents();
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