using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.Database;
using Papyrus.Tests.Infrastructure.Repositories.helpers;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{
    [TestFixture]
    public class SqlTopicRepositoryShouldGet : SqlTest
    {
        private SqlInserter sqlInserter;
        private SqlTopicRepository topicRepository;
        private const string ProductId = "OpportunityId";
        private const string FirstVersionId = "FirstVersionOpportunity";
        private const string SecondVersionId = "SecondVersionOpportunity";

        [SetUp]
        public void Initialize()
        {
            sqlInserter = new SqlInserter(dbConnection);
            topicRepository = new SqlTopicRepository(dbConnection);
            TruncateDataBase().GetAwaiter().GetResult();
        }

        private async Task InsertProductWithItsVersions()
        {
            await InsertProductWithAVersion();
            await InsertProductVersion(SecondVersionId, "2.0", "20150810", ProductId);
        }

        private async Task InsertProductWithAVersion()
        {
            await InsertProduct(ProductId, "Opportunity");
            await InsertProductVersion(FirstVersionId, "1.0", "20150710", ProductId);
        }

        private async Task TruncateDataBase()
        {
            await new DataBaseTruncator(dbConnection).TruncateDataBase();
        }

        [Test]
        public async void a_topics_list_to_show_distincting_by_topic_with_infomation_of_its_last_version()
        {
            await InsertProductWithItsVersions();
            var topic = new Topic(ProductId).WithId("AnyTopicId");
            var firstVersionRange = new VersionRange(FirstVersionId, FirstVersionId).WithId("AnyRangeId");
            var secondVersionRange = new VersionRange(SecondVersionId, SecondVersionId).WithId("AnotherRangeId");
            firstVersionRange.AddDocument(new Document("AnyTitle", "AnyDescription", "AnyContent", "es-ES").WithId("AnyDocumentId"));
            secondVersionRange.AddDocument(new Document("Llamadas Primer mantenimiento", "Explicación", "AnyContent", "es-ES").WithId("AnotherDocumentId"));
            topic.AddVersionRange(firstVersionRange);
            topic.AddVersionRange(secondVersionRange);
            await sqlInserter.Insert(topic);

            var topicSummaries = await topicRepository.GetAllTopicsSummaries();

            topicSummaries.Should().HaveCount(1);
            topicSummaries.Should().Contain(t => t.TopicId == "AnyTopicId" && 
                                               t.Product.ProductName == "Opportunity" &&
                                               t.Product.ProductId == ProductId &&
                                               t.VersionName == "2.0" &&
                                               t.LastDocumentTitle == "Llamadas Primer mantenimiento" &&
                                               t.LastDocumentDescription == "Explicación");
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
        public async Task a_displayable_topic_with_its_versionRanges()
        {
            await InsertProductWithAVersion();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(FirstVersionId, FirstVersionId).WithId("FirstVersionRangeId");
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetEditableTopicById("FirstTopicPapyrusId");

            //TODO: complete test
            var editableVersionRanges = editableTopic.VersionRanges;
            editableVersionRanges.Should().HaveCount(1);
            editableVersionRanges.FirstOrDefault().FromVersion.VersionId.Should().Be(FirstVersionId);
            editableVersionRanges.FirstOrDefault().ToVersion.VersionId.Should().Be(FirstVersionId);
        }

        [Test]
        public async Task a_displayable_topic_with_documents_for_each_of_its_version_ranges()
        {
            await InsertProductWithAVersion();
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(FirstVersionId, FirstVersionId).WithId("FirstVersionRangeId");
            var document = new Document("Título", "Descripción", "Contenido", "es-ES").WithId("DocumentId");
            firstVersionRange.AddDocument(document);
            topic.AddVersionRange(firstVersionRange);
            await sqlInserter.Insert(topic);

            var editableTopic = await topicRepository.GetEditableTopicById("FirstTopicPapyrusId");

            var editableVersionRange = editableTopic.VersionRanges.FirstOrDefault();
            editableVersionRange.Documents.Should().HaveCount(1);
            var editableDocument = editableVersionRange.Documents.First();
            editableDocument.Title.Should().Be("Título");
            editableDocument.Description.Should().Be("Descripción");
            editableDocument.Content.Should().Be("Contenido");
            editableDocument.Language.Should().Be("es-ES");
        }

        protected async Task InsertProductVersion(string versionId, string versionName, string release, string productId)
        {
            await dbConnection.Execute(@"INSERT INTO ProductVersion(VersionId, VersionName, Release, ProductId)
                                            VALUES(@VersionId, @VersionName, @Release, @ProductId);", 
                                            new
                                            {
                                                VersionId = versionId,
                                                VersionName = versionName,
                                                Release = release,
                                                ProductId = productId
                                            });
        }

        protected async Task InsertProduct(string productId, string productName)
        {
            await dbConnection.Execute(@"INSERT INTO Product(ProductId, ProductName) VALUES(@ProductId, @ProductName);",
                                        new
                                        {
                                            ProductId = productId, 
                                            ProductName = productName
                                        });
        }
    }
}