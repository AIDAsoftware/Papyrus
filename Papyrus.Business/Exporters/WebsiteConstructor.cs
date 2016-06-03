using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class WebsiteConstructor {
        private readonly TopicQueryRepository topicRepo;
        private readonly ProductRepository productRepo;
        private WebsiteCollection websitesCollection;

        public WebsiteConstructor(TopicQueryRepository topicRepo, ProductRepository productRepo) {
            this.topicRepo = topicRepo;
            this.productRepo = productRepo;
        }

        public virtual async Task<WebsiteCollection> Construct(PathGenerator pathGenerator, IEnumerable<Product> products, List<string> versions, List<string> languages) {
            websitesCollection = new WebsiteCollection();
            foreach (var product in products) {
                var productWithVersions = await productRepo.GetProductForVersions(product, versions);
                await AddWebsitesFor(pathGenerator, productWithVersions, languages);
            }
            return websitesCollection;
        }

        private async Task AddWebsitesFor(PathGenerator generator, Product product, List<string> languages) {
            foreach (var version in product.Versions) {
                foreach (var language in languages) {
                    RegistToGenerator(generator, product, version, language);
                    var website = await CreateWebsiteWithAllDocumentsFor(generator, product, version, language);
                    if (website.HasNotDocuments()) continue;
                    websitesCollection.Add(generator.GenerateMkdocsPath(), website);
                }
            }
        }

        private async Task<WebSite> CreateWebsiteWithAllDocumentsFor(PathGenerator generator, Product product, ProductVersion version, string language) {
            var documents = await topicRepo.GetAllDocumentsFor(product.Id, version.VersionName, language);
            return new WebSite(documents);
        }

        private static void RegistToGenerator(PathGenerator generator, Product product, ProductVersion version, string language) {
            generator.ForProduct(product.Name);
            generator.ForVersion(version.VersionName);
            generator.ForLanguage(language);
        }
    }
}