using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Exporters;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class PathByProductGeneratorShould {
        [Test]
        public void gets_product_path() {
            var generator = new PathGenerator();
            
            var path = generator.ForLanguage("es-ES")
                .ForProduct("Opportunity")
                .ForVersion("15.11.20")
                .GenerateMkdocsPath();

            path.Should().Be("15.11.20/Opportunity/es-ES");
        }
    }
}