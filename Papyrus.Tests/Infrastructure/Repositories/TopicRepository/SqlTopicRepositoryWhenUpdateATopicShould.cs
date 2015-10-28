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
        private SqlCommandTopicRepository sqlTopicRepository;

        [SetUp]
        public void Initialize()
        {
            sqlInserter = new SqlInserter(dbConnection);
            sqlTopicRepository = new SqlCommandTopicRepository(dbConnection);
        }

        [Test]
        public async Task removes_old_version_ranges_for_given_topic()
        {
            var topic = new Topic("PapyrusId").WithId("TopicId");
            var versionRange = new VersionRange("FirstPapyrusVersionId", "SecondPapyrusVersionId")
                                                .WithId("VersionRangeId");
            topic.AddVersionRange(versionRange);
            await sqlInserter.Insert(topic);

            var topicToUpdate = new Topic("PapyrusId").WithId("TopicId");
            await sqlTopicRepository.Update(topicToUpdate);

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
            var document = new Document("Título", "Descripción", "Contenido", "es-ES").WithId("DocumentId");
            versionRange.AddDocument(document);
            topic.AddVersionRange(versionRange);
            await sqlInserter.Insert(topic);

            var topicToUpdate = new Topic("PapyrusId").WithId("TopicId");
            await sqlTopicRepository.Update(topicToUpdate);

            var oldDocument = (await dbConnection.Query<Document>(@"SELECT Title, Description, Content, Language  
                                            FROM Document 
                                            WHERE DocumentId = @DocumentId",
                                            new { DocumentId = "DocumentId" })).FirstOrDefault();
            oldDocument.Should().BeNull();
        }

        [Test]
        public async Task save_its_new_version_ranges()
        {
            var topic = new Topic("PapyrusId").WithId("TopicId");
            await sqlInserter.Insert(topic);

            topic.AddVersionRange(new VersionRange("FirstVersion", "SecondVersion").WithId("VersionRangeId"));
            await sqlTopicRepository.Update(topic);

            var newVersionRange = (await dbConnection.Query<VersionRange>(@"SELECT FromVersionId, ToVersionId  
                                            FROM VersionRange 
                                            WHERE TopicId = @TopicId",
                                            new { TopicId = "TopicId" })).FirstOrDefault();
            newVersionRange.FromVersionId.Should().Be("FirstVersion");
            newVersionRange.ToVersionId.Should().Be("SecondVersion");
        }

        [Test]
        public async Task save_documents_for_each_of_its_version_ranges()
        {
            var topic = new Topic("PapyrusId").WithId("TopicId");
            await sqlInserter.Insert(topic);

            var versionRange = new VersionRange("FirstVersion", "SecondVersion").WithId("VersionRangeId");
            versionRange.AddDocument(new Document("Título", "Descripción", "Contenido", "es-ES").WithId("DocumentId"));
            topic.AddVersionRange(versionRange);
            await sqlTopicRepository.Update(topic);

            var newVersionRange = (await dbConnection.Query<Document>(@"SELECT Title, Description, Content, Language  
                                                    FROM Document 
                                                    WHERE VersionRangeId = @VersionRangeId",
                                                    new { VersionRangeId = "VersionRangeId" })).FirstOrDefault();
            newVersionRange.Title.Should().Be("Título");
            newVersionRange.Description.Should().Be("Descripción");
            newVersionRange.Content.Should().Be("Contenido");
            newVersionRange.Language.Should().Be("es-ES");
        }
    }
}