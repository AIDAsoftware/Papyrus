using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class WebsiteConstructor {
        private readonly PathGenerator pathGenerator;
        private readonly TopicQueryRepository topicRepo;
        private readonly ProductRepository productRepo;
        private WebsiteCollection websitesCollection;

        public WebsiteConstructor(PathGenerator pathGenerator, TopicQueryRepository topicRepo, ProductRepository productRepo) {
            this.pathGenerator = pathGenerator;
            this.topicRepo = topicRepo;
            this.productRepo = productRepo;
        }

        public async Task<WebsiteCollection> Construct(IEnumerable<Product> products, List<string> versions, List<string> languages) {
            websitesCollection = new WebsiteCollection();
            foreach (var product in products) {
                var productWithVersions = await productRepo.GetProductForVersions(product, versions);
                await AddWebsitesFor(productWithVersions, languages);
            }
            return websitesCollection;
        }

        private async Task AddWebsitesFor(Product product, List<string> languages) {
            foreach (var version in product.Versions) {
                foreach (var language in languages) {
                    RegistToGenerator(product, version, language);
                    var website = await CreateWebsiteWithAllDocumentsFor(product, version, language);
                    if (website.HasNotDocuments()) return;
                    websitesCollection.Add(pathGenerator.GenerateMkdocsPath(), website);
                }
            }
        }

        private async Task<WebSite> CreateWebsiteWithAllDocumentsFor(Product product, ProductVersion version, string language) {
            var documentRoute = pathGenerator.GenerateDocumentRoute();
            var documents = await topicRepo.GetAllDocumentsFor(product.Id, version.VersionName, language, documentRoute);
            var website = new WebSite(RemoveEmptyDocuments(documents));
            return website;
        }

        private static List<ExportableDocument> RemoveEmptyDocuments(List<ExportableDocument> documents) {
            return documents.Where(d => !(d is NoDocument)).ToList();
        }

        private void RegistToGenerator(Product product, ProductVersion version, string language) {
            pathGenerator.ForProduct(product.Name);
            pathGenerator.ForVersion(version.VersionName);
            pathGenerator.ForLanguage(language);
        }
    }
}