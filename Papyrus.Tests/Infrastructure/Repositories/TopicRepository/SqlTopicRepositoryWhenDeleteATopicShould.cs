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
        //   - delete its productVersions from database   
        //   - delete documents for each of its productVersions from database  

        [Test]
        public async Task delete_topic_from_database()
        {
            var topic = new Topic("OpportunityId").WithId("TopicId");
            await new SqlInserter(dbConnection).Insert(topic);
            
            var topicRepository = new SqlTopicRepository(dbConnection);
            await topicRepository.Delete(topic.TopicId);

            var topicFromDataBase = (await dbConnection.Query<object>(@"SELECT * FROM Topic 
                                                                        WHERE TopicId = @TopicId",
                                                                        new {TopicId = topic.TopicId}))
                                                                        .FirstOrDefault();
            topicFromDataBase.Should().BeNull();
        }
    }
}