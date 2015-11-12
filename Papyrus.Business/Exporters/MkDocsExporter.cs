using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private readonly TopicQueryRepository topicRepository;
        private readonly ProductRepository productRepository;
        private const string MkDocsExtension = ".md";

        public MkDocsExporter(TopicQueryRepository topicRepository, ProductRepository productRepository) {
            this.topicRepository = topicRepository;
            this.productRepository = productRepository;
        }

        public async Task ExportDocumentsForProductToFolder(string productId, DirectoryInfo testDirectory)
        {
            var topics = await topicRepository.GetExportableTopicsForProduct(productId);
            foreach (var topic in topics) {
                await topic.ExportTopicIn(testDirectory, MkDocsExtension);
            }
        }

        public async Task ExportDocumentsForProductToFolder(string productId, ProductVersion version, DirectoryInfo directory) {
            var topics = await topicRepository.GetExportableTopicsForProductVersion(productId, version);
            foreach (var topic in topics) {
                var versionDirectory = directory.CreateSubdirectory(version.VersionName);
                var versionRange = topic.VersionRanges.First();
                await versionRange.ExportDocumentForProduct(topic.Product, versionDirectory, MkDocsExtension);
            }
        }

        public async Task ExportAllProductsIn(DirectoryInfo testDirectory) {
            var products = await productRepository.GetAllExportableProducts();
            await ExportAllProductsForLanguage(testDirectory, products, "es-ES");
            await ExportAllProductsForLanguage(testDirectory, products, "en-GB");
        }

        private async Task ExportAllProductsForLanguage(DirectoryInfo testDirectory, List<ExportableProduct> products, string language) {
            var languageDirectory = testDirectory.CreateSubdirectory(language);
            foreach (var product in products) {
                product.Topics = await topicRepository.GetExportableTopicsForProduct(product.ProductId);
                await product.ExportInAllProductsFormatIn(languageDirectory);
            }
        }
    }
}
