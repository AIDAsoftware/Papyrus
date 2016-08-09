using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Tests.Business {
    public class GivenFixture {
        private Documentation documentation = Documentation.WithDocuments(new List<Document>());
        private DocumentsRepository repository;
        private VersionIdentifier version;

        public GivenFixture(DocumentsRepository documentsRepository) {
            repository = documentsRepository;
        }

        public GivenFixture ADocumentationWith(params Document[] documents) {
            documentation = Documentation.WithDocuments(documents.ToList());
            return this;
        }

        private static string AnyUniqueString() {
            return Guid.NewGuid().ToString();
        }

        public static Document ADocument() {
            return new Document(AnyUniqueString(), AnyUniqueString(), AnyUniqueString(), AnyUniqueString(), new VersionIdentifier(AnyUniqueString(), AnyUniqueString()));
        }

        public static VersionIdentifier AVersion() {
            return new VersionIdentifier(AnyUniqueString(), AnyUniqueString());
        }

        public GivenFixture ForVersion(VersionIdentifier givenVersion) {
            version = givenVersion;
            return this;
        }

        public static DocumentDto ADocumentDtoFor(VersionIdentifier version) {
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
            repository.GetDocumentationFor(new VersionIdentifier(version.ProductId, version.VersionId))
                .Returns(documentation);
        }
    }

    public class UncompletedTestContextException : Exception {
        public UncompletedTestContextException(string message) : base(message) {}
    }
}