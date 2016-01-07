using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Infrastructure.Core.Database;
using Papyrus.Tests.Infrastructure.Repositories.Helpers;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{        
    [TestFixture]
    public class SqlTopicRepositoryWhenDeleteATopicShould : SqlTest
    {
        private SqlTopicCommandRepository topicRepository;
        private ProductVersion anyVersion = new ProductVersion("Any", "Any", DateTime.MinValue);
        private ProductVersion anotherVersion = new ProductVersion("Another", "Another", DateTime.MaxValue);

        [SetUp]
        public void Initialize()
        {
            topicRepository = new SqlTopicCommandRepository(dbConnection);
            new DataBaseTruncator(dbConnection).TruncateDataBase().GetAwaiter().GetResult();
        }

        [Test]
        public async Task delete_topic_from_database()
        {
            var topicId = "TopicId";
            var topic = new Topic("OpportunityId").WithId(topicId);
            await new SqlInserter(dbConnection).Insert(topic);
            
            await topicRepository.Delete(topic.TopicId);

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
            var versionRange = new VersionRange(anyVersion, anotherVersion)
                                .WithId(versionRangeId);
            var topic = new Topic("OpportunityId").WithId("TopicId");
            topic.AddVersionRange(versionRange);
            await new SqlInserter(dbConnection).Insert(topic);
            
            await topicRepository.Delete(topic.TopicId);

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
            var versionRange = new VersionRange(anyVersion, anotherVersion)
                                .WithId("VersionRangeId");
            versionRange.AddDocument(document);
            var topic = new Topic("OpportunityId").WithId("TopicId");
            topic.AddVersionRange(versionRange);
            await new SqlInserter(dbConnection).Insert(topic);
            
            await topicRepository.Delete(topic.TopicId);

            var documentFromDataBase = (await dbConnection.Query<object>(@"SELECT * FROM Document 
                                                                        WHERE DocumentId = @DocumentId",
                                                                        new {DocumentId = documentId}))
                                                                        .FirstOrDefault();
            documentFromDataBase.Should().BeNull();
        }
    }
}