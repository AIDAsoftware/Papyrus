using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class MkdocsExporterShould
    {
        // TODO:
        //   - create a folder for spanish and english documents
        //   - insert in each of these folders topics its respective topics
        [Test]
        public async Task create_two_folders_for_spanish_and_english_documents_into_the_given_directory()
        {
            var testDirectory = Directory.CreateDirectory("test");
            var topicRepository = Substitute.For<TopicRepository>();
            var mkdocsExporter = new MkDocsExporter(topicRepository);

            await mkdocsExporter.ExportDocumentsForProductToFolder("PapyrusId", testDirectory);

            var languagesFolders = testDirectory.GetDirectories().Select(d => d.Name);
            languagesFolders.Should().Contain("Español");
            languagesFolders.Should().Contain("English");
        }
    }
}