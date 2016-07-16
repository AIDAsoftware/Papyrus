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
        private MkDocsExporter exporter;
        private readonly List<string> languages = new List<string>{"es-ES", "en-GB"};
        private readonly WebSite WebsiteWithADocument = WebsiteWith(new ExportableDocument("Title", "content"));
        private const string OpportunityId = "OpportunityId";

        [SetUp]
        public void SetUp() {
            topicRepo = Substitute.For<TopicQueryRepository>();
            productRepo = Substitute.For<ProductRepository>();
            websiteConstructor = Substitute.For<WebsiteConstructor>(topicRepo, productRepo);
            exporter = Substitute.For<MkDocsExporter>(new object[] {null});
        }

        [Test]
        public async Task export_all_documentation_for_selected_product_in_spanish_and_english() {
            StubOutProductRepoToReturnAsAllProducts(allProducts);
            StubOutProductRepoToReturnAsAllVersionsWhenIsCalledWithProduct(versions, OpportunityId);
            var viewModel = await InitializeTopicGridVMWith(topicRepo, productRepo, exporter, websiteConstructor);
            viewModel.SelectedProduct = viewModel.Products.First(p => p.ProductId == OpportunityId);
            var websiteCollection = new WebsiteCollection { WebsiteWithADocument};
            List<DisplayableProduct> products = new List<DisplayableProduct>{ viewModel.SelectedProduct };
            WhenWebConstructorIsCalledWith(products.First(), languages).Returns(Task.FromResult(websiteCollection));

            await ExecuteExportSelectedProductCommandFrom(viewModel);

            exporter.Received().Export(WebsiteWithADocument, 
                        Arg.Is<ConfigurationPaths>(c =>c.ImagesFolder == "ImagesFolderForTests"));
        }

        private void StubOutProductRepoToReturnAsAllVersionsWhenIsCalledWithProduct(List<string> versions, string productId) {
            productRepo.GetAllVersionsFor(productId).Returns(
                Task.FromResult(versions.Select(v => new ProductVersion(v, v, DateTime.Today)).ToList()));
        }

        private static async Task ExecuteExportSelectedProductCommandFrom(TopicsGridVm viewModel) {
            await viewModel.ExportProductToMkDocs.ExecuteAsync(new object());
        }

        private async Task<TopicsGridVm> InitializeTopicGridVMWith(TopicQueryRepository topicRepo, ProductRepository productRepo, MkDocsExporter exporter, WebsiteConstructor constructor) {
            var viewModel = new TopicsGridVm(topicRepo, productRepo, exporter, constructor);
            await viewModel.Initialize();
            return viewModel;
        }

        private async Task<WebsiteCollection> WhenWebConstructorIsCalledWith(DisplayableProduct product, List<string> languages)
        {
            return await websiteConstructor.Construct(
                Arg.Is<Product>(p => p.Id == product.ProductId), Arg.Is<List<string>>(ls => ls.SequenceEqual(languages)));
        }

        private static WebSite WebsiteWith(ExportableDocument document) {
            var webSite = new WebSite(new List<ExportableDocument> {
                document
            }, "AnyProductName");
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