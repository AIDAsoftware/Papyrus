using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Exporters;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class PathByVersionGeneratorShould {
        [Test]
        public void gets_version_path() {
            var generator = new PathByVersionGenerator();

            var path = generator.ForLanguage("es-ES")
                .ForVersion("15.11.20")
                .GenerateMkdocsPath();

            path.Should().Be("es-ES/15.11.20");
        }

        [Test]
        public void gets_product_as_document_route() {
            var generator = new PathByVersionGenerator();
            var documentRoute = generator.ForProduct("Product").GenerateDocumentRoute();
            documentRoute.Should().Be("Product");
        }
    }
}