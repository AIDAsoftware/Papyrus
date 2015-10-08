using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories
{
    [TestFixture]
    public class SqlTopicRepositoryShould : SqlTest
    {
        // TODO:
        //   save a document with all of its version ranges

        [SetUp]
        public async void TruncateDataBase()
        {
            await dbConnection.Execute("TRUNCATE TABLE Topic;");
            await dbConnection.Execute("TRUNCATE TABLE Product;");
            await dbConnection.Execute("TRUNCATE TABLE ProductVersion;");
            await dbConnection.Execute("TRUNCATE TABLE VersionRange;");
            await dbConnection.Execute("TRUNCATE TABLE Document;");
        }

        [Test]
        public async void get_a_list_with_all_topics_distincting_by_topic_with_infomation_of_its_last_version()
        {
            await InsertTopic("AnyTopicId", "AnyProductId");
            await InsertProduct("AnyProductId", "Opportunity");
            await InsertProductVersion("FirstVersionOpportunity", "1.0", "20150801", "AnyProductId");
            await InsertProductVersion("SecondVersionOpportunity", "2.0", "20150810", "AnyProductId");
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

        [Test]
        public async void save_a_topic()
        {
            var productId = "OpportunityId";
            await InsertProduct(productId, "Opportunity");
            var versionId = "FirstVersionId";
            await InsertProductVersion(versionId, "1.0", "20150710", productId);
            var topic = new Topic().WithId("AnyTopicId").ForProduct(productId);
            var versionRange = new VersionRange(fromVersionId: versionId, toVersionId: versionId).WithId("AnyVersionRangeId");
            topic.AddVersionRange(versionRange);
            versionRange.AddDocument("es-ES", 
                new Document2("AnyTitle", "AnyDescription", "AnyContent").WithId("AnyDocumentId")
            );
            
            var topicRepository = new SqlTopicRepository(dbConnection);
            await topicRepository.Save(topic);

            var topicFromRepo = await GetTopicById("AnyTopicId");
            Assert.That(topicFromRepo.ProductId, Is.EqualTo(productId));
            var versionRangeFromRepo = await GetVersionRangeById("AnyVersionRangeId");
            Assert.That(versionRangeFromRepo.FromVersionId, Is.EqualTo("FirstVersionId"));
            Assert.That(versionRangeFromRepo.ToVersionId, Is.EqualTo("FirstVersionId"));
            Assert.That(versionRangeFromRepo.TopicId, Is.EqualTo("AnyTopicId"));
            var documentFromRepo = await GetDocumentById("AnyDocumentId");
            Assert.That(documentFromRepo.Title, Is.EqualTo("AnyTitle"));
            Assert.That(documentFromRepo.Description, Is.EqualTo("AnyDescription"));
            Assert.That(documentFromRepo.Content, Is.EqualTo("AnyContent"));
            Assert.That(documentFromRepo.Language, Is.EqualTo("es-ES"));
            Assert.That(documentFromRepo.VersionRangeId, Is.EqualTo("AnyVersionRangeId"));
        }

        private async Task<dynamic> GetDocumentById(string id)
        {
            return (await dbConnection.Query<dynamic>(@"SELECT Title, Description, Content, Language, VersionRangeId 
                                                        FROM Document 
                                                        WHERE DocumentId = @DocumentId;",
                                                        new { DocumentId = id }))
                                                        .FirstOrDefault();
        }

        private async Task<dynamic> GetTopicById(string topicId)
        {
            return (await dbConnection.Query<dynamic>(@"SELECT TopicId, ProductId FROM Topic
                                                        WHERE TopicId = @TopicId;",
                                                        new {TopicId = topicId})).FirstOrDefault();
        }

        private async Task<dynamic> GetVersionRangeById(string id)
        {
            return (await dbConnection.Query<dynamic>(@"SELECT FromVersionId, ToVersionId, TopicId FROM VersionRange
                                                        WHERE VersionRangeId = @VersionRangeId;",
                                                        new {VersionRangeId = id}))
                                                        .FirstOrDefault();
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

        private async Task InsertProductVersion(string versionId, string versionName, string release, string productId)
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

        private async Task InsertProduct(string productId, string productName)
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