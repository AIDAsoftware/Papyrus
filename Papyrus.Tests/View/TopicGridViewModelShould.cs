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
        private readonly List<string> versions = new List<string> {
            "15.11.27"
        };
        private readonly List<DisplayableProduct> allProducts = new List<DisplayableProduct> {
            new DisplayableProduct {ProductId = OpportunityId, ProductName = "Any"}
        };

        private TopicQueryRepository topicRepo;
        private ProductRepository productRepo;
        private WebsiteConstructor websiteConstructor;
        private MkdocsExporter exporter;
        private readonly List<string> languages = new List<string>{"es-ES", "en-GB"};
        private readonly WebSite WebsiteWithADocument = WebsiteWith(new ExportableDocument("Title", "content", ""));
        private const string OpportunityId = "OpportunityId";

        [SetUp]
        public void SetUp() {
            topicRepo = Substitute.For<TopicQueryRepository>();
            productRepo = Substitute.For<ProductRepository>();
            websiteConstructor = Substitute.For<WebsiteConstructor>(topicRepo, productRepo);
            exporter = Substitute.For<MkdocsExporter>();
        }

        [Test]
        public async Task export_All_documentation_for_all_products_in_spanish_and_english() {
            StubOutProductRepoToReturnAsAllVersions(versions);
            StubOutProductRepoToReturnAsAllProducts(allProducts);
            var websiteCollection = new WebsiteCollection {{"Any/Path", WebsiteWithADocument}};
            WhenWebConstructorIsCalledWith(
                Arg.Any<PathByVersionGenerator>(), allProducts, versions, languages
            ).Returns(Task.FromResult(websiteCollection));
            var viewModel = await InitializeTopicGridVMWith(topicRepo, productRepo, exporter, websiteConstructor);

            await ExecuteExportAllProductsCommandFrom(viewModel);

            exporter.Received().Export(WebsiteWithADocument, "Any/Path");
        }

        private static async Task ExecuteExportAllProductsCommandFrom(TopicsGridVM viewModel) {
            await viewModel.ExportAllProducts.ExecuteAsync(new object());
        }

        private async Task<TopicsGridVM> InitializeTopicGridVMWith(TopicQueryRepository topicRepo, ProductRepository productRepo, MkdocsExporter exporter, WebsiteConstructor constructor) {
            var viewModel = new TopicsGridVM(topicRepo, productRepo, exporter, constructor);
            await viewModel.Initialize();
            return viewModel;
        }

        private async Task<WebsiteCollection> WhenWebConstructorIsCalledWith(PathByVersionGenerator aPathByVersionGenerator, List<DisplayableProduct> products, List<string> versions, List<string> languages) {
            return await websiteConstructor.Construct(
                aPathByVersionGenerator, 
                Arg.Is<IEnumerable<Product>>(ps => AreEquivalent(ps, products)),
                versions, 
                Arg.Is<List<string>>(ls => ls.SequenceEqual(languages)));
        }

        private bool AreEquivalent(IEnumerable<Product> products, List<DisplayableProduct> displayableProducts) {
            return products
                .Select(x => x.Id)
                .SequenceEqual(displayableProducts
                                .Select(x => x.ProductId));
        }

        private static WebSite WebsiteWith(ExportableDocument document) {
            var webSite = new WebSite(new List<ExportableDocument> {
                document
            });
            return webSite;
        }

        private void StubOutProductRepoToReturnAsAllProducts(List<DisplayableProduct> products) {
            productRepo.GetAllDisplayableProducts().Returns(Task.FromResult(products));
        }

        private void StubOutProductRepoToReturnAsAllVersions(List<string> versions) {
            productRepo.GetAllVersionsNamesDistinctingByName().Returns(Task.FromResult(versions));
        }
    }
}