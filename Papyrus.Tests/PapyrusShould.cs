using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
    }

    public class GetDocumentation {
        public DocumentsRepository DocumentsRepository { get; set; }

        public GetDocumentation(DocumentsRepository documentsRepository) {
            DocumentsRepository = documentsRepository;
        }

        public List<Document> ExecuteFor(string productId, string versionId) {
            return DocumentsRepository.GetDocumentationFor(productId, versionId).ToList();
        }
    }

    public interface DocumentsRepository {
        Documentation GetDocumentationFor(string productId, string versionId);
    }

    public class Documentation {
        public void AddDocuments(List<Document> documents) {
            Documents = documents;
        }

        public List<Document> Documents { get; set; }


        public List<Document> ToList() {
            return Documents;
        }
    }

    public class Document {
        public Document(string title, string description, string content, string language) {
            
        }
    }
}