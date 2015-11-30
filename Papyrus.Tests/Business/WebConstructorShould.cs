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
        // TODO:
        //   - should construct a website with documents associated to given product, version and language
        //   - should get the route to construct the website in the website collection using the given strategy

        [Test]
        public async Task construct_a_website_with_documents_associated_to_given_product_and_version() {
            var pathGenerator = Substitute.For<PathGenerator>();
            pathGenerator.GenerateMkdocsPath().Returns("Route/Route"); 
            pathGenerator.GenerateDocumentRoute().Returns("DocumentRoute"); 
            var topicRepo = Substitute.For<TopicQueryRepository>();
            var productRepo = Substitute.For<ProductRepository>();
            var versionsNames = new List<string>{ "15.11.27" };
            var versions = new List<ProductVersion>{ new ProductVersion("AnyId", "15.11.27", DateTime.Today) };
            var languages = new List<string>{ "es-ES" };
            var opportunity = new Product("OpportunityId", "Opportunity", versions);
            productRepo.GetProductForVersions("OpportunityId", versionsNames)
                        .Returns(Task.FromResult(opportunity));
            var document = new ExportableDocument("Title", "Content", "DocumentRoute");
            topicRepo.GetAllDocumentsFor(opportunity, languages).Returns(Task.FromResult(new List<ExportableDocument>{ document }));
            var websiteConstructor = new WebsiteConstructor(pathGenerator, topicRepo, productRepo);

            var websites = await websiteConstructor.Export(new List<string>{opportunity.Id}, versionsNames, languages);

            ExportableDocument websiteDocument = websites["Route/Route"].Documents.First();
            websiteDocument.Content.Should().Be("Content");
            websiteDocument.Title.Should().Be("Title");
            websiteDocument.Route.Should().Be("DocumentRoute");
        }
    }
}