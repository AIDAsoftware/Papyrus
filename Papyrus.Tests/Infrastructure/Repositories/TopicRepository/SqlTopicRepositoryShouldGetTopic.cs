using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.Database;
using Papyrus.Tests.Infrastructure.Repositories.helpers;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{
    [TestFixture]
    public class SqlTopicRepositoryShouldGetTopic : SqlTest
    {
        private const string ProductId = "OpportunityId";
        private const string FirstVersionId = "FirstVersionId";

        [SetUp]
        public async void InitializeDataBase()
        {
            await TruncateDataBase();
            await InsertOpportunityAsProductAndItsFirstVersion();
        }

        private async Task TruncateDataBase()
        {
            await new DataBaseTruncator(dbConnection).TruncateDataBase();
        }

        [Test]
        public async void list_to_show_distincting_by_topic_with_infomation_of_its_last_version()
        {
            await InsertProductVersion("SecondVersionOpportunity", "2.0", "20150810", ProductId);
            await InsertTopic("AnyTopicId", ProductId);
            await InsertVersionRange("AnyRangeId", "FirstVersionOpportunity", "FirstVersionOpportunity", "AnyTopicId");
            await InsertVersionRange("AnotherRangeId", "SecondVersionOpportunity", "SecondVersionOpportunity", "AnyTopicId");
            await InsertDocument("PrimerMantenimientoOpportunity", "Primer Mantenimiento", "Explicación",
                                "Puedes llamar a los clientes que necesitan...", "es-ES", "AnyRangeId");
            await InsertDocument("PrimerMantenimientoOpportunityVersion2", "Llamadas Primer mantenimiento", "Explicación",
                                "Puedes llamar a los clientes que necesitan...", "es-ES", "AnotherRangeId");


            var topicRepository = new SqlTopicRepository(dbConnection);

            var topicsToShow = await topicRepository.GetAllTopicsToShow();

            topicsToShow.Should().HaveCount(1);
            topicsToShow.Should().Contain(t => t.TopicId == "AnyTopicId" && 
                                               t.ProductName == "Opportunity" &&
                                               t.VersionName == "2.0" &&
                                               t.LastDocumentTitle == "Llamadas Primer mantenimiento" &&
                                               t.LastDocumentDescription == "Explicación");
        }

        private async Task InsertOpportunityAsProductAndItsFirstVersion()
        {
            await InsertProduct(ProductId, "Opportunity");
            await InsertProductVersion(FirstVersionId, "1.0", "20150710", ProductId);
        }

        private async Task InsertDocument(string documentId, string title, string description, string content, string language, string rangeId)
        {
            await dbConnection.Execute(@"INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
                                            VALUES(@DocumentId, @Title, @Description, @Content, @Language, @RangeId);",
                                            new
                                            {
                                                DocumentId = documentId,
                                                Title = title,
                                                Description = description,
                                                Content = content,
                                                Language = language,
                                                RangeId = rangeId
                                            });
        }

        private async Task InsertVersionRange(string rangeId, string fromVersionId, string toVersionId, string topicId)
        {
            await dbConnection.Execute(@"INSERT INTO VersionRange(VersionRangeId, FromVersionId, ToVersionId, TopicId)
                                            VALUES(@VersionRangeId, @FromVersionId, @ToVersionId, @TopicId);",
                                            new
                                            {
                                                VersionRangeId = rangeId,
                                                FromVersionId = fromVersionId,
                                                ToVersionId = toVersionId,
                                                TopicId = topicId
                                            });
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

        private async Task InsertTopic(string topicId, string productId)
        {
            await dbConnection.Execute(@"INSERT INTO Topic (TopicId, ProductId) VALUES (@TopicId, @ProductId);", 
                                        new
                                        {
                                            TopicId = topicId,
                                            ProductId = productId
                                        });
        }
    }
}