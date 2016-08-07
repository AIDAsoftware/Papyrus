using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business;

namespace Papyrus.Tests {
    // TODO : Alias to get semantic
    [TestFixture]
    public class PapyrusShould {
        private DocumentsRepository repository;
        private GivenFixture given;

        [SetUp]
        public void SetUp() {
            repository = Substitute.For<DocumentsRepository>();
            given = new GivenFixture(repository);
        }

        [Test] //TODO: unify Given and GivenFixture and hide repo
        public void get_the_documentation_for_a_given_product_and_version() {
            var document = GivenFixture.ADocument();
            var version = GivenFixture.AVersion();
            given.ADocumentationWith(document)
                .ForVersion(version)
                .CreateContext();

            var documentation = GetDocumentationFor(version);

            documentation.Should().HaveCount(1);
            documentation.First().ShouldBeEquivalentTo(document);
        }

        [Test]
        public void create_document_for_a_given_product_and_version() {
            var version = GivenFixture.AVersion();
            var documentDto = GivenFixture.ADocumentDtoFor(version);

            ExecuteCreateDocument(documentDto, version.ProductId, version.VersionId);

            repository.Received(1).CreateDocumentFor(documentDto.AsDocument(), version.ProductId, version.VersionId);
        }

        private void ExecuteCreateDocument(DocumentDto documentDto, string productId, string versionId) {
            var createDocument = new CreateDocument(repository);
            createDocument.ExecuteFor(documentDto: documentDto, productId: productId, versionId: versionId);
        }

        private List<Document> GetDocumentationFor(TestProductVersion version) {
            var getDocumentation = new GetDocumentation(repository);
            return getDocumentation.ExecuteFor(version.ProductId, version.VersionId);
        }
    }
}