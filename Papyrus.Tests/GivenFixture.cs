using System;
using System.Linq;
using NSubstitute;
using Papyrus.Business;

namespace Papyrus.Tests {
    public class GivenFixture {
        private Documentation documentation = new Documentation();
        private DocumentsRepository repository;
        private TestProductVersion version;

        public GivenFixture InRepository(DocumentsRepository givenRepository) {
            repository = givenRepository;
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

        public GivenFixture ForVersion(TestProductVersion givenVersion) {
            version = givenVersion;
            return this;
        }

        public static DocumentDto ADocumentDtoFor(TestProductVersion version) {
            return new DocumentDto {
                Title = AnyUniqueString(),
                Description = AnyUniqueString(),
                Content = AnyUniqueString(),
                Language = AnyUniqueString(),
                ProductId = version.ProductId,
                VersionId = version.VersionId
            };
        }

        public void CreateContext() {
            if (repository == null || version == null)
                throw new UncompletedTestContextException("Repository or Version is not set");
            repository
                .GetDocumentationFor(productId: version.ProductId, versionId: version.VersionId)
                .Returns(documentation);
        }
    }

    public class UncompletedTestContextException : Exception {
        public UncompletedTestContextException(string message) : base(message) {}
    }
}