using System.Collections.Generic;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Tests.Builders {
    public class VersionRangeBuilder {
        private readonly string id;
        private readonly ProductVersion firstVersion;
        private readonly ProductVersion lastVersion;
        private List<Document> documents;

        public VersionRangeBuilder(string id, ProductVersion firstVersion, ProductVersion lastVersion) {
            this.id = id;
            this.firstVersion = firstVersion;
            this.lastVersion = lastVersion;
            this.documents = new List<Document>();
        }

        public VersionRangeBuilder WithDocuments(params Document[] documents) {
            this.documents.AddRange(documents);
            return this;
        }

        public VersionRange Build() {
            var versionRange = new VersionRange(firstVersion, lastVersion).WithId(id);
            foreach (var document in documents) {
                versionRange.AddDocument(document);
            }
            return versionRange;
        }
    }
}