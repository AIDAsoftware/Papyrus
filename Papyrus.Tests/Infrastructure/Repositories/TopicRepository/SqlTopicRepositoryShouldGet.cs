using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const string ProductId = "OpportunityId";
        private const string FirstVersionId = "FirstVersionOpportunity";
        private const string SecondVersionId = "SecondVersionOpportunity";

        [SetUp]
        public async void InitializeDataBase()
        {
            await TruncateDataBase();
            await InsertProduct(ProductId, "Opportunity");
            await InsertProductVersion(FirstVersionId, "1.0", "20150710", ProductId);
            await InsertProductVersion(SecondVersionId, "2.0", "20150810", ProductId);
        }

        private async Task TruncateDataBase()
        {
            await new DataBaseTruncator(dbConnection).TruncateDataBase();
        }

        [Test]
        public async void a_topics_list_to_show_distincting_by_topic_with_infomation_of_its_last_version()
        {
            var topic = new Topic(ProductId).WithId("AnyTopicId");
            var firstVersionRange = new VersionRange(FirstVersionId, FirstVersionId).WithId("AnyRangeId");
            var secondVersionRange = new VersionRange(SecondVersionId, SecondVersionId).WithId("AnotherRangeId");
            firstVersionRange.AddDocument("es-ES", new Document2("AnyTitle", "AnyDescription", "AnyContent").WithId("AnyDocumentId"));
            secondVersionRange.AddDocument("es-ES", new Document2("Llamadas Primer mantenimiento", "Explicación", "AnyContent").WithId("AnotherDocumentId"));
            topic.AddVersionRange(firstVersionRange);
            topic.AddVersionRange(secondVersionRange);
            await Insert(topic);


            var topicRepository = new SqlTopicRepository(dbConnection);

            var topicsToList = await topicRepository.GetAllTopicsToList();

            topicsToList.Should().HaveCount(1);
            topicsToList.Should().Contain(t => t.TopicId == "AnyTopicId" && 
                                               t.ProductName == "Opportunity" &&
                                               t.VersionName == "2.0" &&
                                               t.LastDocumentTitle == "Llamadas Primer mantenimiento" &&
                                               t.LastDocumentDescription == "Explicación");
        }

        // TODO
        //   get a topic with its VersionRanges
        //   get a topic with Documents for its VersionRanges

        [Test]
        public async Task a_displayable_topic_with_its_product()
        {
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            await Insert(topic);

            var editableTopic = await new SqlTopicRepository(dbConnection).GetEditableTopicById("FirstTopicPapyrusId");

            editableTopic.Product.ProductId.Should().Be(ProductId);
            editableTopic.Product.ProductName.Should().Be("Opportunity");
        }

        [Test]
        public async Task a_displayable_topic_with_its_versionRanges()
        {
            var topic = new Topic(ProductId).WithId("FirstTopicPapyrusId");
            var firstVersionRange = new VersionRange(FirstVersionId, FirstVersionId).WithId("FirstVersionRangeId");
            firstVersionRange.AddDocument("es-ES", new Document2("Title", "Description", "Content").WithId("DocumentId"));
            topic.AddVersionRange(firstVersionRange);
            await Insert(topic);

            var editableTopic = await new SqlTopicRepository(dbConnection).GetEditableTopicById("FirstTopicPapyrusId");

            var editableVersionRanges = editableTopic.VersionRanges;
            editableVersionRanges.Should().HaveCount(1);
            editableVersionRanges.FirstOrDefault().FromVersionId.Should().Be(FirstVersionId);
            editableVersionRanges.FirstOrDefault().ToVersionId.Should().Be(FirstVersionId);
        }


        private async Task Insert(Topic topic)
        {
            await dbConnection.Execute("INSERT INTO Topic(TopicId, ProductId) VALUES (@TopicId, @ProductId)",
                new
                {
                    TopicId = topic.TopicId,
                    ProductId = topic.ProductId
                });
            foreach (var versionRange in topic.VersionRanges)
            {
                await InsertVersionRangeForTopic(versionRange, topic);
            }
        }

        private async Task InsertVersionRangeForTopic(VersionRange versionRange, Topic topic)
        {
            await dbConnection.Execute(@"INSERT INTO VersionRange(VersionRangeId, FromVersionId, ToVersionId, TopicId)
                                                VALUES (@VersionRangeId, @FromVersionId, @ToVersionId, @TopicId)",
                new
                {
                    VersionRangeId = versionRange.VersionRangeId,
                    FromVersionId = versionRange.FromVersionId,
                    ToVersionId = versionRange.ToVersionId,
                    TopicId = topic.TopicId
                });
            foreach (var document in versionRange.Documents)
            {
                await InsertDocumentForVersionRange(document, versionRange);
            }
        }

        private async Task InsertDocumentForVersionRange(LanguageDocumentPair languageDocumentPair, VersionRange versionRange)
        {
            await
                dbConnection.Execute(
                    @"INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
                                                    VALUES(@DocumentId, @Title, @Description, @Content, @Language, @VersionRangeId);",
                    new
                    {
                        DocumentId = languageDocumentPair.Document.DocumentId,
                        Title = languageDocumentPair.Document.Title,
                        Description = languageDocumentPair.Document.Description,
                        Content = languageDocumentPair.Document.Content,
                        Language = languageDocumentPair.Language,
                        VersionRangeId = versionRange.VersionRangeId
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
    }
}