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
            var topics = await repository.GetExportableTopicsForProduct(productId);
            foreach (var topic in topics) {
                await topic.ExportTopicIn(testDirectory, MkDocsExtension);
            }
        }

        public async Task ExportDocumentsForProductToFolder(string productId, ProductVersion version, DirectoryInfo directory) {
            var topics = await repository.GetExportableTopicsForProductVersion(productId, version);
            foreach (var topic in topics) {
                var versionDirectory = directory.CreateSubdirectory(version.VersionName);
                var versionRange = topic.VersionRanges.First();
                await versionRange.ExportDocumentForProduct(topic.Product, versionDirectory, MkDocsExtension);
            }
        }
    }
}
