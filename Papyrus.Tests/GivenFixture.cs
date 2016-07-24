using System;
using System.Linq;
using NSubstitute;
using Papyrus.Business;

namespace Papyrus.Tests {
    public class GivenFixture {
        private Documentation documentation;
        private DocumentsRepository repository;

        public GivenFixture InRepository(DocumentsRepository givenRepository) {
            repository = givenRepository;
            return this;
        }

        public GivenFixture ForVersion(string productId, string versionId) {
            repository
                .GetDocumentationFor(productId: "myProductId", versionId: "myVersionId")
                .Returns(documentation);
            return this;
        }

        public GivenFixture ADocumentationWith(params Document[] documents) {
            documentation = new Documentation();
            documentation.AddDocuments(documents.ToList());
            return this;
        }

        private static string AnyUniqueString() {
            return Guid.NewGuid().ToString();
        }

        public static Document ADocument() {
            return new Document(AnyUniqueString(), AnyUniqueString(), AnyUniqueString(), AnyUniqueString());
        }

        public static TestProductVersion AVersion() {
            return new TestProductVersion(AnyUniqueString(), AnyUniqueString());
        }

        public GivenFixture ForVersion(TestProductVersion version) {
            repository
                .GetDocumentationFor(productId: version.ProductId, versionId: version.VersionId)
                .Returns(documentation);
            return this;
        }
    }
}