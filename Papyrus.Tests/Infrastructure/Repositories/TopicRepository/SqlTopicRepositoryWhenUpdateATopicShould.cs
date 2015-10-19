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
        private SqlInserter sqlInserter;

        [SetUp]
        public void Initialize()
        {
            sqlInserter = new SqlInserter(dbConnection);
        }

        // TODO:
        //  - save all new version ranges for given topic
        //  - save all documents for each of given topic's version ranges

        [Test]
        public async Task removes_old_version_ranges_for_given_topic()
        {
            var topic = new Topic("PapyrusId").WithId("TopicId");
            var versionRange = new VersionRange("FirstPapyrusVersionId", "SecondPapyrusVersionId")
                                                .WithId("VersionRangeId");
            topic.AddVersionRange(versionRange);
            await sqlInserter.Insert(topic);

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
            versionRange.AddDocument(document);
            topic.AddVersionRange(versionRange);
            await sqlInserter.Insert(topic);

            var topicToUpdate = new Topic("PapyrusId").WithId("TopicId");
            await new SqlTopicRepository(dbConnection).Update(topicToUpdate);

            var oldDocument = (await dbConnection.Query<Document2>(@"SELECT Title, Description, Content, Language  
                                            FROM Document 
                                            WHERE DocumentId = @DocumentId",
                                            new { DocumentId = "DocumentId" })).FirstOrDefault();
            oldDocument.Should().BeNull();
        }
    }
}