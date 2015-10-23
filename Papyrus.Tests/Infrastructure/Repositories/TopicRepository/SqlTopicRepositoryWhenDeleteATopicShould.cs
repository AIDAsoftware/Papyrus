using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{        
    [TestFixture]
    public class SqlTopicRepositoryWhenDeleteATopicShould : SqlTest
    {
        // TODO: 
        //   - delete documents for each of its productVersions from database  

        [Test]
        public async Task delete_topic_from_database()
        {
            var topicId = "TopicId";
            var topic = new Topic("OpportunityId").WithId(topicId);
            await new SqlInserter(dbConnection).Insert(topic);
            
            var topicRepository = new SqlTopicRepository(dbConnection);
            await topicRepository.Delete(topic);

            var topicFromDataBase = (await dbConnection.Query<object>(@"SELECT * FROM Topic 
                                                                        WHERE TopicId = @TopicId",
                                                                        new {TopicId = topicId}))
                                                                        .FirstOrDefault();
            topicFromDataBase.Should().BeNull();
        }

        [Test]
        public async Task delete_its_product_versions_from_database()
        {
            var versionRangeId = "VersionRangeId";
            var versionRange = new VersionRange("AnyProductVersionId", "AnotherProductVersionId")
                                .WithId(versionRangeId);
            var topic = new Topic("OpportunityId").WithId("TopicId");
            topic.AddVersionRange(versionRange);
            await new SqlInserter(dbConnection).Insert(topic);
            
            var topicRepository = new SqlTopicRepository(dbConnection);
            await topicRepository.Delete(topic);

            var versionFromDataBase = (await dbConnection.Query<object>(@"SELECT * FROM VersionRange 
                                                                        WHERE VersionRangeId = @VersionRangeId",
                                                                        new { VersionRangeId = versionRangeId}))
                                                                        .FirstOrDefault();
            versionFromDataBase.Should().BeNull();
        }

        [Test]
        public async Task delete_all_documents_for_each_of_its_product_versions_from_database()
        {
            var documentId = "DocumentId";
            var document = new Document("Título", "Descripción", "Contenido", "es-ES").WithId(documentId);
            var versionRange = new VersionRange("AnyProductVersionId", "AnotherProductVersionId")
                                .WithId("VersionRangeId");
            versionRange.AddDocument(document);
            var topic = new Topic("OpportunityId").WithId("TopicId");
            topic.AddVersionRange(versionRange);
            await new SqlInserter(dbConnection).Insert(topic);
            
            var topicRepository = new SqlTopicRepository(dbConnection);
            await topicRepository.Delete(topic);

            var documentFromDataBase = (await dbConnection.Query<object>(@"SELECT * FROM Document 
                                                                        WHERE DocumentId = @DocumentId",
                                                                        new {DocumentId = documentId}))
                                                                        .FirstOrDefault();
            documentFromDataBase.Should().BeNull();
        }
    }
}