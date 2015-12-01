using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class WebConstructorShould {
        private TopicQueryRepository topicRepo;
        private ProductRepository productRepo;
        private PathGenerator pathGenerator;
        private WebsiteConstructor websiteConstructor;
        // TODO:
        //   - should construct a website with documents associated to given product, version and language
        //   - should get the route to construct the website in the website collection using the given strategy

        [SetUp]
        public void SetUp() {
            topicRepo = Substitute.For<TopicQueryRepository>();
            productRepo = Substitute.For<ProductRepository>();
            pathGenerator = Substitute.For<PathGenerator>();
            websiteConstructor = new WebsiteConstructor(pathGenerator, topicRepo, productRepo);
        }
        
        [Test]
        public async Task construct_a_website_with_documents_associated_to_given_product_and_version() {
            pathGenerator.GenerateMkdocsPath().Returns("Route/Route"); 
            pathGenerator.GenerateDocumentRoute().Returns("DocumentRoute"); 
            var versionsNames = new List<string>{ "15.11.27" };
            var versions = new List<ProductVersion>{ new ProductVersion("AnyId", "15.11.27", DateTime.Today) };
            var languages = new List<string>{ "es-ES" };
            var opportunity = new Product("OpportunityId", "Opportunity", versions);
            productRepo.GetProductForVersions("OpportunityId", versionsNames)
                        .Returns(Task.FromResult(opportunity));
            var document = new ExportableDocument("Title", "Content", "DocumentRoute");
            topicRepo.GetAllDocumentsFor(opportunity, "15.11.27", "es-ES").Returns(Task.FromResult(new List<ExportableDocument> { document }));

            var websites = await websiteConstructor.Construct(new List<string>{opportunity.Id}, versionsNames, languages);

            ExportableDocument websiteDocument = websites["Route/Route"].Documents.First();
            websiteDocument.Content.Should().Be("Content");
            websiteDocument.Title.Should().Be("Title");
            websiteDocument.Route.Should().Be("DocumentRoute");
        }
        
        [Test]
        public async Task construct_proper_keys_for_each_website() {
            var versionsNames = new List<string>{ "15.11.27" };
            var versions = new List<ProductVersion>{ new ProductVersion("AnyId", "15.11.27", DateTime.Today) };
            var languages = new List<string>{ "es-ES" };
            var opportunity = new Product("OpportunityId", "Opportunity", versions);
            productRepo.GetProductForVersions("OpportunityId", versionsNames)
                        .Returns(Task.FromResult(opportunity));
            var document = new ExportableDocument("Title", "Content", "DocumentRoute");
            topicRepo.GetAllDocumentsFor(opportunity, "15.11.27", "es-ES").Returns(Task.FromResult(new List<ExportableDocument>{ document }));

            await websiteConstructor.Construct(new List<string>{opportunity.Id}, versionsNames, languages);

            pathGenerator.Received().ForProduct(opportunity.Name);
            pathGenerator.Received().ForVersion("15.11.27");
            pathGenerator.Received().ForLanguage("es-ES");
        }
    }
}