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
        [Test]
        public void get_the_documentation_for_a_given_product_and_version() {
            var documentation = new Documentation();
            var document = new Document("a title", "a description", "a content", "es-ES");
            documentation.AddDocuments(new List<Document>() {
                document
            });
            var repository = Substitute.For<DocumentsRepository>();
            repository.GetDocumentationFor(productId: "myProductId", versionId: "myVersionId")
                .Returns(documentation);
            var getDocumentation = new GetDocumentation(repository);

            var givenDocumentation = getDocumentation.ExecuteFor("myProductId", "myVersionId");

            givenDocumentation.Should().HaveCount(1);
            givenDocumentation.First().ShouldBeEquivalentTo(document);
        }

        [Test]
        public void create_document_for_a_given_product_and_version() {
            var repository = Substitute.For<DocumentsRepository>();
            
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
    }

    internal static class TestExtensionMethodsForDocumentDto {
        public static Document AsDocument(this DocumentDto documentDto) {
            return Arg.Is<Document>(d => 
                d.Title == documentDto.Title &&
                d.Description == documentDto.Description &&
                d.Content == documentDto.Content &&
                d.Language == documentDto.Language
            );
        }
    }

    public class DocumentDto {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public string ProductId { get; set; }
        public string VersionId { get; set; }
    }

    public class CreateDocument {
        private DocumentsRepository DocumentsRepository { get; }

        public CreateDocument(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public void ExecuteFor(DocumentDto documentDto) {
            DocumentsRepository.CreateDocumentFor(ToDocument(documentDto), documentDto.ProductId, documentDto.VersionId);
        }

        private static Document ToDocument(DocumentDto document) {
            return new Document(document.Title, document.Description, document.Content, document.Language);
        }
    }
}