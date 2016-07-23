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
    }
}