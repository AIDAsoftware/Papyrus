using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{
    [TestFixture]
    public class SqlTopicRepositoryWhenUpdateATopicShould : SqlTest
    {
        // TODO:
        //  - removes old related versionRanges for given topic
        //  - removes old related documents for given topic
        //  - save all new version ranges for given topic
        //  - save all documents for each of given topic's version ranges

        [Test]
        public async Task removes_old_version_ranges_for_given_topic()
        {
            var topic = new Topic("PapyrusId").WithId("TopicId");
            var versionRange = new VersionRange("FirstPapyrusVersionId", "SecondPapyrusVersionId")
                                                .WithId("VersionRangeId");
            topic.AddVersionRange(versionRange);
            await Insert(topic);

            var topicToUpdate = new Topic("PapyrusId").WithId("TopicId");
            await new SqlTopicRepository(dbConnection).Update(topicToUpdate);

            var oldVersionRange = (await dbConnection.Query<VersionRange>(@"SELECT FromVersionId, ToVersionId  
                                            FROM VersionRange 
                                            WHERE VersionRangeId = @VersionRangeId",
                                            new {VersionRangeId = "VersionRangeId"})).FirstOrDefault();
            oldVersionRange.Should().BeNull();
        }

        [Test]
        public async Task removes_old_documents_for_given_topic()
        {
            var topic = new Topic("PapyrusId").WithId("TopicId");
            var versionRange = new VersionRange("FirstPapyrusVersionId", "SecondPapyrusVersionId")
                                                .WithId("VersionRangeId");
            var document = new Document2("Título", "Descripción", "Contenido", "es-ES").WithId("DocumentId");
            versionRange.AddDocument("es-ES", document);
            topic.AddVersionRange(versionRange);
            await Insert(topic);

            var topicToUpdate = new Topic("PapyrusId").WithId("TopicId");
            await new SqlTopicRepository(dbConnection).Update(topicToUpdate);

            var oldDocument = (await dbConnection.Query<Document2>(@"SELECT Title, Description, Content  
                                            FROM Document 
                                            WHERE DocumentId = @DocumentId",
                                            new { DocumentId = "DocumentId" })).FirstOrDefault();
            oldDocument.Should().BeNull();
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

        private async Task InsertDocumentForVersionRange(Document2 document, VersionRange versionRange)
        {
            await
                dbConnection.Execute(
                    @"INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
                                                    VALUES(@DocumentId, @Title, @Description, @Content, @Language, @VersionRangeId);",
                    new
                    {
                        DocumentId = document.DocumentId,
                        Title = document.Title,
                        Description = document.Description,
                        Content = document.Content,
                        Language = document.Language,
                        VersionRangeId = versionRange.VersionRangeId
                    });
        }
    }
}