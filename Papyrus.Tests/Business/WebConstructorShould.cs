using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class WebConstructorShould {
        private const string EnglishContent = "Content";
        private const string EnglishTitle = "Title";
        private const string DocumentRoute = "DocumentRoute";
        private TopicQueryRepository topicRepo;
        private ProductRepository productRepo;
        private PathGenerator pathGenerator;
        private WebsiteConstructor websiteConstructor;
        private string Spanish = "es-ES";
        private string LastVersionName = "15.11.27";
        private readonly ExportableDocument englishDocument = new ExportableDocument(EnglishTitle, EnglishContent, DocumentRoute);

        [SetUp]
        public void SetUp() {
            topicRepo = Substitute.For<TopicQueryRepository>();
            productRepo = Substitute.For<ProductRepository>();
            pathGenerator = Substitute.For<PathGenerator>();
            websiteConstructor = new WebsiteConstructor(topicRepo, productRepo);
        }
        
        [Test]
        public async Task construct_a_website_with_documents_associated_to_given_product_and_version() {
            var versionsNames = VersionNames(LastVersionName);
            var opportunity = new Product("OpportunityId", "Opportunity", VersionsFrom(versionsNames));
            StubPathGeneratorToReturnAs(exportationPath: "Route/Route", documentRoute: DocumentRoute);
            RepositoryReturnsProductWhenAskingForVersions(opportunity, versionsNames);
            topicRepo.GetAllDocumentsFor(opportunity.Id, LastVersionName, Spanish, DocumentRoute)
                .Returns(AsyncDocumentsList(englishDocument));

            var websites = await websiteConstructor
                .Construct(pathGenerator, ProductsList(opportunity), versionsNames, Languages(Spanish));

            ExportableDocument websiteDocument = websites["Route/Route"].First().Documents.First();
            websiteDocument.Content.Should().Be(EnglishContent);
            websiteDocument.Title.Should().Be(EnglishTitle);
            websiteDocument.Route.Should().Be(DocumentRoute);
        }

        [Test]
        public async Task construct_proper_keys_for_each_website() {
            var versionsNames = VersionNames(LastVersionName);
            var opportunity = new Product("OpportunityId", "Opportunity", VersionsFrom(versionsNames));
            StubPathGeneratorToReturnAs(documentRoute: DocumentRoute, exportationPath:"AnyPath");
            RepositoryReturnsProductWhenAskingForVersions(opportunity, versionsNames);
            topicRepo.GetAllDocumentsFor(opportunity.Id, LastVersionName, Spanish, DocumentRoute)
                .Returns(AsyncDocumentsList(englishDocument));

            await websiteConstructor.Construct(pathGenerator, ProductsList(opportunity), versionsNames, Languages(Spanish));

            pathGenerator.Received().ForProduct(opportunity.Name);
            pathGenerator.Received().ForVersion(LastVersionName);
            pathGenerator.Received().ForLanguage(Spanish);
        }

        private void StubPathGeneratorToReturnAs(string documentRoute) {
            StubPathGeneratorToReturnAs(documentRoute: documentRoute, exportationPath: null);
        }

        [Test]
        public async Task not_create_website_when_there_are_no_document_available() {
            var versionsNames = VersionNames(LastVersionName);
            var opportunity = new Product("OpportunityId", "Opportunity", VersionsFrom(versionsNames));
            StubPathGeneratorToReturnAs(documentRoute: DocumentRoute);
            RepositoryReturnsProductWhenAskingForVersions(opportunity, versionsNames);
            topicRepo.GetAllDocumentsFor(opportunity.Id, LastVersionName, Spanish, DocumentRoute)
                .Returns(AsyncDocumentsList(new NoDocument()));

            var websites = await websiteConstructor.Construct(pathGenerator, ProductsList(opportunity), versionsNames, Languages(Spanish));

            pathGenerator.DidNotReceive().GenerateMkdocsPath();
            websites.Count.Should().Be(0);
        }

        private List<ProductVersion> VersionsFrom(List<string> versionsNames) {
            return versionsNames.Select(versionsName => 
                    new ProductVersion(versionsName, versionsName, DateTime.Today))
                .ToList();
        }

        private async static Task<List<ExportableDocument>> AsyncDocumentsList(ExportableDocument document) {
            return new List<ExportableDocument>{ document };
        }


        private void RepositoryReturnsProductWhenAskingForVersions(Product opportunity, List<string> versionsNames) {
            productRepo.GetProductForVersions(opportunity, versionsNames)
                .Returns(Task.FromResult(opportunity));
        }

        private List<string> Languages(string language) {
            return new List<string> { language };
        }

        private static List<string> VersionNames(params string[] languages) {
            return languages.ToList();
        }

        private static List<Product> ProductsList(Product opportunity) {
            return new List<Product> { opportunity };
        }

        private void StubPathGeneratorToReturnAs(string exportationPath, string documentRoute) {
            pathGenerator.GenerateMkdocsPath().Returns(exportationPath);
            pathGenerator.GenerateDocumentRoute().Returns(documentRoute);
        }
    }
}