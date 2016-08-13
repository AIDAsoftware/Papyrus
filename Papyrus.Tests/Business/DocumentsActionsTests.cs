using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using Papyrus.Business.Actions;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;
// TODO : Separate Build and Given in two classes
using Build = Papyrus.Tests.Business.GivenFixture;
using Given = Papyrus.Tests.Business.GivenFixture;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class DocumentsActionsTests {
        private DocumentsRepository documentsRepository;
        private Given given;

        [SetUp]
        public void SetUp() {
            documentsRepository = Substitute.For<DocumentsRepository>();
            given = new Given(documentsRepository);
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

            documentsRepository.Received(1).CreateDocumentFor(documentDto.Equivalent());
        }

        private void ExecuteCreateDocument(DocumentDto documentDto) {
            var createDocument = new CreateDocument(documentsRepository);
            createDocument.ExecuteFor(documentDto);
        }

        private List<Document> GetDocumentationFor(VersionIdentifier version) {
            var getDocumentation = new GetDocumentation(documentsRepository);
            return getDocumentation.ExecuteFor(version.ProductId, version.VersionId).ToList();
        }
    }
}