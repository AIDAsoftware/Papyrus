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

        public virtual async Task<WebsiteCollection> Construct(IEnumerable<Product> products, List<string> versions, List<string> languages) {
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
                    var website = await CreateWebsiteWithAllDocumentsFor(product, version, language);
                    if (website.HasNotDocuments()) continue;
                    websitesCollection.Add(website);
                }
            }
        }

        private async Task<WebSite> CreateWebsiteWithAllDocumentsFor(Product product, ProductVersion version, string language) {
            var documents = await topicRepo.GetAllDocumentsFor(product.Id, version.VersionName, language);
            return new WebSite(documents, product.Name, language, version.VersionName);
        }
    }
}