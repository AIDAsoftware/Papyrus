using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Documents;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Features.Topics;

namespace Papyrus.Tests.View {
    
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

            exporter.Received().Export(WebsiteWithADocument, Arg.Is<string>(x => x.EndsWith("Any/Path")));
        }
        
        [Test]
        public async Task export_all_documentation_for_selected_product_in_spanish_and_english() {
            StubOutProductRepoToReturnAsAllProducts(allProducts);
            StubOutProductRepoToReturnAsAllVersionsWhenIsCalledWithProduct(versions, OpportunityId);
            var viewModel = await InitializeTopicGridVMWith(topicRepo, productRepo, exporter, websiteConstructor);
            viewModel.SelectedProduct = viewModel.Products.First(p => p.ProductId == OpportunityId);
            var websiteCollection = new WebsiteCollection { { "Any/Path", WebsiteWithADocument } };
            WhenWebConstructorIsCalledWith(
                Arg.Any<PathByProductGenerator>(), new List<DisplayableProduct>{ viewModel.SelectedProduct }, versions, languages
            ).Returns(Task.FromResult(websiteCollection));

            await ExecuteExportSelectedProductCommandFrom(viewModel);

            exporter.Received().Export(WebsiteWithADocument, Arg.Is<string>(x => x.EndsWith("Any/Path")));
        }
        
        [Test]
        public async Task export_documentation_for_last_version_of_selected_product_in_spanish_and_english() {
            StubOutProductRepoToReturnAsAllProducts(allProducts);
            StubOutProductRepoToReturnAsLastVersionWhenIsCalledWithProduct("15.11.27", OpportunityId);
            var viewModel = await InitializeTopicGridVMWith(topicRepo, productRepo, exporter, websiteConstructor);
            viewModel.SelectedProduct = viewModel.Products.First(p => p.ProductId == OpportunityId);
            var websiteCollection = new WebsiteCollection { { "Any/Path", WebsiteWithADocument } };
            WhenWebConstructorIsCalledWith(
                Arg.Any<PathByProductGenerator>(), new List<DisplayableProduct>{ viewModel.SelectedProduct }, 
                new List<string>{"15.11.27"}, languages
            ).Returns(Task.FromResult(websiteCollection));

            await ExecuteExportLastVersionForSelectedProductCommandFrom(viewModel);

            exporter.Received().Export(WebsiteWithADocument, Arg.Is<string>(x => x.EndsWith("Any/Path")));
        }

        private static async Task ExecuteExportLastVersionForSelectedProductCommandFrom(TopicsGridVM viewModel) {
            await viewModel.ExportLastVersionToMkDocs.ExecuteAsync(new object());
        }

        private void StubOutProductRepoToReturnAsLastVersionWhenIsCalledWithProduct(string lastVersion, string productId) {
            productRepo.GetLastVersionForProduct(productId).Returns(Task.FromResult(new ProductVersion(lastVersion, lastVersion, DateTime.Today)));
        }

        private void StubOutProductRepoToReturnAsAllVersionsWhenIsCalledWithProduct(List<string> versions, string productId) {
            productRepo.GetAllVersionsFor(productId).Returns(
                Task.FromResult(versions.Select(v => new ProductVersion(v, v, DateTime.Today)).ToList()));
        }

        private static async Task ExecuteExportSelectedProductCommandFrom(TopicsGridVM viewModel) {
            await viewModel.ExportProductToMkDocs.ExecuteAsync(new object());
        }

        private static async Task ExecuteExportAllProductsCommandFrom(TopicsGridVM viewModel) {
            await viewModel.ExportAllProducts.ExecuteAsync(new object());
        }

        private async Task<TopicsGridVM> InitializeTopicGridVMWith(TopicQueryRepository topicRepo, ProductRepository productRepo, MkdocsExporter exporter, WebsiteConstructor constructor) {
            var viewModel = new TopicsGridVM(topicRepo, productRepo, exporter, constructor);
            await viewModel.Initialize();
            return viewModel;
        }

        private async Task<WebsiteCollection> WhenWebConstructorIsCalledWith(PathGenerator aPathByVersionGenerator, List<DisplayableProduct> products, List<string> versions, List<string> languages) {
            return await websiteConstructor.Construct(
                aPathByVersionGenerator, 
                Arg.Is<IEnumerable<Product>>(ps => AreEquivalent(ps, products)),
                Arg.Is<List<string>>(vs => vs.SequenceEqual(versions)), 
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
            productRepo.GetAllVersionNames().Returns(Task.FromResult(versions));
        }
    }
}