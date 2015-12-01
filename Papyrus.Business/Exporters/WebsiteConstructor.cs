using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class WebsiteConstructor {
        private readonly PathGenerator pathGenerator;
        private readonly TopicQueryRepository topicRepo;
        private readonly ProductRepository productRepo;

        public WebsiteConstructor(PathGenerator pathGenerator, TopicQueryRepository topicRepo, ProductRepository productRepo) {
            this.pathGenerator = pathGenerator;
            this.topicRepo = topicRepo;
            this.productRepo = productRepo;
        }

        public async Task<WebsiteCollection> Construct(IEnumerable<string> products, List<string> versions, List<string> languages) {
            var websitesCollection = new WebsiteCollection();
            foreach (var productId in products) {
                var product = await productRepo.GetProductForVersions(productId, versions);
                pathGenerator.ForProduct(product.Name);
                foreach (var version in product.Versions) {
                    pathGenerator.ForVersion(version.VersionName);
                    foreach (var language in languages) {
                        var documents = await topicRepo.GetAllDocumentsFor(product, version.VersionName, language);
                        pathGenerator.ForLanguage(language);
                        var website = new WebSite(documents);
                        websitesCollection.Add(pathGenerator.GenerateMkdocsPath(), website);  
                    }
                }
            }
            return websitesCollection;
        }
    }
}