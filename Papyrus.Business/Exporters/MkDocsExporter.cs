using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private readonly TopicQueryRepository repository;
        private const string MkDocsExtension = ".md";

        public MkDocsExporter(TopicQueryRepository repository) {
            this.repository = repository;
        }

        public async Task ExportDocumentsForProductToFolder(string productId, DirectoryInfo testDirectory)
        {
            var topics = await repository.GetEditableTopicsForProduct(productId);
            foreach (var topic in topics)
            {
                await ExportTopic(topic, testDirectory, MkDocsExtension);
            }
        }

        private async Task ExportTopic(ExportableTopic topic, DirectoryInfo directory, string extension)
        {
            await topic.ExportTopicIn(directory, extension);
        }

        public async Task ExportDocumentsForProductToFolder(string productId, ProductVersion version, DirectoryInfo directory) {
            var topics = await repository.GetEditableTopicsForProductVersion(productId, version);
            foreach (var exportableVersionRange in topics.Select(exportableTopic => exportableTopic.VersionRanges.First())) {
                await exportableVersionRange.CreateDocumentsStructureForEachLanguageIn(directory, MkDocsExtension);
            }
        }
    }
}
