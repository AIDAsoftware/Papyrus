using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.View {
    
    //TODO:
    // - export all documentation for all products
    // - export only last version for selected product
    // - export all documentation for selected product
    
    [TestFixture]
    public class TopicGridViewModelShould {
        [Test]
        public async Task export_All_documentation_for_all_products_in_spanish_and_english() {
            var topicRepo = Substitute.For<TopicQueryRepository>();
            var productRepo = Substitute.For<ProductRepository>();
            var allVersions = new List<string> {
                "15.11.27"
            };
            productRepo.GetAllVersionsNamesDistinctingByName().Returns(Task.FromResult(allVersions));
            productRepo.GetAllDisplayableProducts().Returns(Task.FromResult(new List<DisplayableProduct> {
                new DisplayableProduct {ProductId = "OpportunityId", ProductName = "Any"}
            }));
            var exporter = Substitute.For<MkdocsExporter>();
            var pathGenerator = Substitute.For<PathGenerator>();
            var websiteConstructor = Substitute.For<WebsiteConstructor>(pathGenerator, topicRepo, productRepo);
            var websiteCollection = new WebsiteCollection();
            var webSite = new WebSite(new List<ExportableDocument> {
                new ExportableDocument("Title", "content", "")
            });
            websiteCollection.Add("Any/Path", webSite);
            websiteConstructor.Construct(Arg.Is<IEnumerable<Product>>(ps => ps.Any(p => p.Id == "OpportunityId")), allVersions, Arg.Is<List<string>>(ls => ls.Contains("es-ES") && ls.Contains("en-GB") && ls.Count == 2)).Returns(Task.FromResult(websiteCollection));
            var viewModel = new TopicsGridVM(topicRepo, productRepo, exporter, websiteConstructor);
            await viewModel.Initialize();

            await viewModel.ExportAllProducts.ExecuteAsync(new object());

            exporter.Received().Export(webSite, "Any/Path");
        }
    }
}