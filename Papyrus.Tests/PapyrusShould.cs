using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Papyrus.Business;

namespace Papyrus.Tests {

    /*
     * TODO
     *  - Get the documentation given a product and a version
     *  - Create a document for a given product and version
     */


    [TestFixture]
    public class PapyrusShould {
        private DocumentsRepository repository;
        private GivenFixture given;

        [SetUp]
        public void SetUp() {
            repository = Substitute.For<DocumentsRepository>();
        }

        [Test]
        public void get_the_documentation_for_a_given_product_and_version() {
            var document = GivenFixture.ADocument();
            var version = GivenFixture.AVersion();
            given.ADocumentationWith(document)
                .ForVersion(version)
                .InRepository(repository);

            var givenDocumentation = GetDocumentationFor(version);

            givenDocumentation.Should().HaveCount(1);
            givenDocumentation.First().ShouldBeEquivalentTo(document);
        }

        [Test]
        public void create_document_for_a_given_product_and_version() {
            var createDocument = new CreateDocument(repository);

            const string anyProductid = "Any ProductID";
            const string anyVersionid = "Any VersionID";
            var documentDto = new DocumentDto {
                Title = "Any Title",
                Description = "Any Description",
                Content = "Any Content",
                Language = "Any Language",
                ProductId = anyProductid,
                VersionId = anyVersionid
            };
            createDocument.ExecuteFor(documentDto: documentDto);

            repository.Received(1).CreateDocumentFor(documentDto.AsDocument(), anyProductid, anyVersionid);
        }

        private List<Document> GetDocumentationFor(TestProductVersion version) {
            var getDocumentation = new GetDocumentation(repository);
            var givenDocumentation = getDocumentation.ExecuteFor(version.ProductId, version.VersionId);
            return givenDocumentation;
        }
    }
}