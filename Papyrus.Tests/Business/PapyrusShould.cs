using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Actions;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;
// TODO : Separate Build and Given in two classes
using Build = Papyrus.Tests.Business.GivenFixture;
using Given = Papyrus.Tests.Business.GivenFixture;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class PapyrusShould {
        private DocumentsRepository repository;
        private Given given;

        [SetUp]
        public void SetUp() {
            repository = Substitute.For<DocumentsRepository>();
            given = new Given(repository);
        }

        [Test]
        public void get_the_documentation_for_a_given_product_and_version() {
            var document = Build.ADocument();
            var version = Build.AVersion();
            given.ADocumentationWith(document)
                .ForVersion(version)
                .CreateContext();

            var documentation = GetDocumentationFor(version);

            documentation.Single().ShouldBeEquivalentTo(document);
        }

        [Test]
        public void create_document_for_a_given_product_and_version() {
            var version = Build.AVersion();
            var documentDto = Build.ADocumentDtoFor(version);

            ExecuteCreateDocument(documentDto);

            repository.Received(1).CreateDocumentFor(documentDto.Equivalent());
        }

        private void ExecuteCreateDocument(DocumentDto documentDto) {
            var createDocument = new CreateDocument(repository);
            createDocument.ExecuteFor(documentDto);
        }

        private List<Document> GetDocumentationFor(VersionIdentifier version) {
            var getDocumentation = new GetDocumentation(repository);
            return getDocumentation.ExecuteFor(version.ProductId, version.VersionId).ToList();
        }
    }
}