using System;
using System.Collections.Generic;
using System.Linq;
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
        //   it should disctinct by topic Id showing the title and description for the last document added to a Topic

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
        public async void get_a_list_with_all_topics_to_show()
        {
            await InsertTopic("AnyTopicId", "AnyProductId");
            await InsertProduct("AnyProductId", "Opportunity");
            await InsertProductVersion("FirstVersionOpportunity", "1.0", "20150801", "AnyProductId");
            await InsertVersionRange("AnyRangeId", "FirstVersionOpportunity", "FirstVersionOpportunity", "AnyTopicId");
            await InsertDocument("PrimerMantenimientoOpportunity", "Primer Mantenimiento", "Explicación",
                                "Puedes llamar a los clientes que necesitan...", "es-ES", "AnyRangeId");

            var topicRepository = new SqlTopicRepository(dbConnection);

            var topicsToShow = await topicRepository.GetAllTopicsToShow();

            topicsToShow.Should().HaveCount(1);
            topicsToShow.Should().Contain(t => t.TopicId == "AnyTopicId" && 
                                                t.ProductName == "Opportunity" && 
                                                t.LastDocumentTitle == "Primer Mantenimiento" &&
                                                t.LastDocumentDescription == "Explicación");
        }

        [Test]
        public async void filter_the_loaded_list_to_show_only_one_row_for_a_topic_showing_its_last_document()
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
                                                t.LastDocumentTitle == "Llamadas Primer mantenimiento" &&
                                                t.LastDocumentDescription == "Explicación");
        }

        [Test]
        public void xxx()
        {
            var dates = new List<DateTime>();
            dates.Add(DateTime.Today.AddDays(-30));
            dates.Add(DateTime.Today);
            dates.Add(DateTime.Today.AddDays(-20));
            var orderedDates = dates.OrderByDescending(d => d);
            orderedDates.First().ShouldBeEquivalentTo(DateTime.Today);
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