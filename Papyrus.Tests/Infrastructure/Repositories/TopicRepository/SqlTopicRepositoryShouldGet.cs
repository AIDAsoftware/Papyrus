using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Infrastructure.Core.Database;
using Papyrus.Tests.Builders;
using Papyrus.Tests.Infrastructure.Repositories.Helpers;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{
    [TestFixture]
    public class SqlTopicRepositoryShouldGet : SqlTest
    {
        private SqlInserter sqlInserter;
        private SqlTopicQueryRepository topicRepository;
        private const string EnglishLanguage = "en-GB";
        private const string SpanishLanguage = "es-ES";
        private const string ProductId = "OpportunityId";
        private const string FirstVersionId = "FirstVersionOpportunity";
        private const string FirstVersionName = "1.0";
        private const string SecondVersionId = "SecondVersionOpportunity";
        private const string SecondVersionName = "2.0";
        private Document spanishDocument;
        private Document englishDocument;
        private static ProductVersion version2 = new ProductVersion(SecondVersionId, SecondVersionName, DateTime.Today.AddDays(-10));
        private ProductVersion version1 = new ProductVersion(FirstVersionId, FirstVersionName, DateTime.Today.AddDays(-20));

        [SetUp]
        public void Initialize()
        {
            sqlInserter = new SqlInserter(dbConnection);
            topicRepository = new SqlTopicQueryRepository(dbConnection);
            TruncateDataBase().GetAwaiter().GetResult();
            spanishDocument = new Document("Título", "Descripción", "Contenido", SpanishLanguage);
            englishDocument = new Document("Title", "Description", "Content", EnglishLanguage);
        }

        [Test]
        public async Task a_topic_summary_list()
        {
            await InsertProductWithItsVersions();
            var topic = new TopicBuilder(ProductId, "AnyTopicId")
                                .WithVersionRanges(
                                    new VersionRangeBuilder("AnyRangeId", version1, version1)
                                        .WithDocuments(
                                            new Document("Título", "Descripción", "Any", "es-ES").WithId("AnyDocumentId"))
                                ).Build();
            await sqlInserter.Insert(topic);

            var topicSummaries = await topicRepository.GetAllTopicsSummariesFor("es-ES");

            topicSummaries.Should().Contain(t => t.TopicId == "AnyTopicId" && 
                                               t.Product.ProductName == "Opportunity" &&
                                               t.Product.ProductId == ProductId &&
                                               t.VersionName == FirstVersionName &&
                                               t.LastDocumentTitle == "Título" &&
                                               t.LastDocumentDescription == "Descripción");
        }

        [Test]
        public async Task a_topic_summary_list_distincting_by_topic_showing_last_valid_version_name() {
            await InsertProductWithItsVersions();
            var version3 = new ProductVersion("thirdVersion", "3.0", DateTime.Today);
            await InsertProductVersion(version3, ProductId);
            var topic = new TopicBuilder(ProductId, "AnyTopicId")
                                .WithVersionRanges(
                                    new VersionRangeBuilder("FirstRange", version1, version2)
                                            .WithDocuments(
                                                spanishDocument.WithId("FirstDocument")),
                                    new VersionRangeBuilder("SecondRange", version3, version3)
                                            .WithDocuments(
                                                new Document("Any", "Any", "Any", "es-ES").WithId("ThirdDocument"))
                                ).Build();
            await sqlInserter.Insert(topic);

            var topicSummaries = await topicRepository.GetAllTopicsSummariesFor("es-ES");

            topicSummaries.Should().Contain(t => t.VersionName == "3.0");
        }

        [Test]
        public async Task a_list_with_topics_with_wildcard_as_to_version()
        {
            await InsertProductWithItsVersions();
            var topic = new Topic(ProductId).WithId("AnyTopicId");
            var firstVersionRange = new VersionRange(version1, new LastProductVersion()).WithId("AnyRangeId");
            firstVersionRange.AddDocument(spanishDocument.WithId("AnyDocumentId"));
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var topicSummaries = await topicRepository.GetAllTopicsSummariesFor("es-ES");

            topicSummaries.First().VersionName.Should().Be(LastProductVersion.Name);
        }

        [Test]
        public async Task a_displayable_topic_with_its_product()
        {
            await InsertProduct(ProductId, "Opportunity");

            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetEditableTopicById("FirstTopicPapyrusId");

            editableTopic.Product.ProductId.Should().Be(ProductId);
            editableTopic.Product.ProductName.Should().Be("Opportunity");
        }

        [Test]
        public async Task a_topic()
        {
            await InsertProduct(ProductId, "Opportunity");
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetTopicById("FirstTopicPapyrusId");

            editableTopic.ProductId.Should().Be(ProductId);
        }

        [Test]
        public async Task a_displayable_topic_with_its_versionRanges()
        {
            await InsertProductWithAVersion();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetEditableTopicById("FirstTopicPapyrusId");

            var editableVersionRanges = editableTopic.VersionRanges;
            editableVersionRanges.Should().HaveCount(1);
            editableVersionRanges.First().FromVersion.VersionId.Should().Be(FirstVersionId);
            editableVersionRanges.First().FromVersion.VersionName.Should().Be(FirstVersionName);
            editableVersionRanges.First().ToVersion.VersionId.Should().Be(FirstVersionId);
            editableVersionRanges.First().ToVersion.VersionName.Should().Be(FirstVersionName);
        }

        [Test]
        public async Task a_topic_with_its_versionRanges()
        {
            await InsertProductWithAVersion();
            var topicId = "FirstTopicOpportunityId";
            var topic = new Topic(ProductId).WithId(topicId);
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetTopicById(topicId);

            var editableVersionRanges = editableTopic.VersionRanges;
            editableVersionRanges.Should().HaveCount(1);
            editableVersionRanges.First().FromVersion.VersionId.Should().Be(FirstVersionId);
            editableVersionRanges.First().FromVersion.VersionName.Should().Be(FirstVersionName);
            editableVersionRanges.First().ToVersion.VersionId.Should().Be(FirstVersionId);
            editableVersionRanges.First().ToVersion.VersionName.Should().Be(FirstVersionName);
        }

        [Test]
        public async Task a_displayable_topic_with_documents_for_each_of_its_version_ranges()
        {
            await InsertProductWithAVersion();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            spanishDocument.WithId("DocumentId");
            firstVersionRange.AddDocument(spanishDocument);
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetEditableTopicById("FirstTopicPapyrusId");

            var editableVersionRange = editableTopic.VersionRanges.First();
            editableVersionRange.Documents.Should().HaveCount(1);
            var editableDocument = editableVersionRange.Documents.First();
            editableDocument.Title.Should().Be("Título");
            editableDocument.Description.Should().Be("Descripción");
            editableDocument.Content.Should().Be("Contenido");
            editableDocument.Language.Should().Be(SpanishLanguage);
        }

        [Test]
        public async Task a_topic_with_documents_for_each_of_its_version_ranges()
        {
            await InsertProductWithAVersion();
            var topicToInsert = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            spanishDocument.WithId("DocumentId");
            firstVersionRange.AddDocument(spanishDocument);
            topicToInsert.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topicToInsert);

            var topic = await topicRepository.GetTopicById("FirstTopicPapyrusId");

            var versionRange = topic.VersionRanges.First();
            versionRange.Documents.Should().HaveCount(1);
            var editableDocument = versionRange.Documents[SpanishLanguage];
            editableDocument.Title.Should().Be("Título");
            editableDocument.Description.Should().Be("Descripción");
            editableDocument.Content.Should().Be("Contenido");
            editableDocument.Language.Should().Be(SpanishLanguage);
        }

        [Test]
        public async Task editable_topic_when_to_version_is_a_wildcard() {
            await InsertProduct(ProductId, "any Product");
            var fromVersionId = "anyId";
            await InsertProductVersion(version1, ProductId);
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, new LastProductVersion()).WithId("FirstVersionRangeId");
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetEditableTopicById("FirstTopicPapyrusId");

            (editableTopic.VersionRanges.First().ToVersion is LastProductVersion).Should().BeTrue();
        }

        [Test]
        public async Task exportable_topic_for_a_given_version_and_language() {
            await InsertProductWithItsVersions();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            var secondVersionRange = new VersionRange(version2, new LastProductVersion()).WithId("SecondVersionRangeId");
            firstVersionRange.AddDocument(spanishDocument.WithId("AnyId"));
            secondVersionRange.AddDocument(new Document("Título", "Descripción", "Contenido", "es-ES").WithId("AnotherId"));
            topic.AddVersionRange(firstVersionRange);
            topic.AddVersionRange(secondVersionRange);
            await sqlInserter.Insert(topic);

            var documents = await topicRepository.GetAllDocumentsFor(ProductId, version1.VersionName, SpanishLanguage);

            documents.Should().HaveCount(1);
            var document = documents.First();
            document.Title.Should().Be(spanishDocument.Title);
            document.Content.Should().Be(spanishDocument.Content);
        }
        
        [Test]
        public async Task get_empty_list_if_there_are_no_documents() {
            await InsertProductWithItsVersions();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            firstVersionRange.AddDocument(spanishDocument.WithId("AnyId"));
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var documents = await topicRepository.GetAllDocumentsFor(ProductId, version1.VersionName, EnglishLanguage);

            documents.Should().HaveCount(0);
        }
        
        [Test]
        public async Task get_empty_exportable_document_list_if_there_given_version_does_not_exist() {
            await InsertProductWithItsVersions();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(version1, version1).WithId("FirstVersionRangeId");
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var documents = await topicRepository.GetAllDocumentsFor(ProductId, "No existing Version", EnglishLanguage);

            documents.Should().HaveCount(0);
        }

        private async Task InsertProductWithItsVersions() {
            await InsertProductWithAVersion();
            await InsertProductVersion(version2, ProductId);
        }

        private async Task InsertProductWithAVersion() {
            await InsertProduct(ProductId, "Opportunity");
            await InsertProductVersion(version1, ProductId);
        }

        private async Task TruncateDataBase() {
            await new DataBaseTruncator(dbConnection).TruncateDataBase();
        }

        protected async Task InsertProductVersion(ProductVersion version, string productId) {
            await dbConnection.Execute(@"INSERT INTO ProductVersion(VersionId, VersionName, Release, ProductId)
                                            VALUES(@VersionId, @VersionName, @Release, @ProductId);",
                new {
                    VersionId = version.VersionId,
                    VersionName = version.VersionName,
                    Release = version.Release,
                    ProductId = productId
                });
        }

        protected async Task InsertProduct(string productId, string productName) {
            await dbConnection.Execute(@"INSERT INTO Product(ProductId, ProductName) VALUES(@ProductId, @ProductName);",
                new {
                    ProductId = productId,
                    ProductName = productName
                });
        }
    }
}