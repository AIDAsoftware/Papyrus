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
        //   get all topics to show with properties "last version range document name", "description of such document", "product"

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
            await dbConnection.Execute(@"INSERT INTO Topic (TopicId, ProductId) VALUES ('AnyTopicId', 'AnyProductId');

                                        INSERT INTO Product(ProductId, ProductName) VALUES('AnyProductId', 'Opportunity');
            
                                        INSERT INTO ProductVersion(VersionId, VersionName, Release, ProductId)
                                            VALUES('FirstVersionOpportunity', '1.0', '20150801', 'AnyProductId');
            
                                        INSERT INTO VersionRange(VersionRangeId, FromVersionId, ToVersionId, TopicId)
                                            VALUES('AnyRangeId', 'FirstVersionOpportunity', 'FirstVersionOpportunity', 'AnyTopicId');
            
                                        INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
                                            VALUES('PrimerMantenimientoOpportunity', 'Primer Mantenimiento', 'Explicación',
                                                'Puedes llamar a los clientes que necesitan...', 'es-ES', 'AnyRangeId');");
            var topicRepository = new SqlTopicRepository(dbConnection);

            var topicsToShow = await topicRepository.GetAllTopicsToShow();

            topicsToShow.Should().HaveCount(1);
            topicsToShow.Should().Contain(t => t.TopicId == "AnyTopicId" && 
                                                t.ProductName == "Opportunity" && 
                                                t.LastDocumentTitle == "Primer Mantenimiento" &&
                                                t.LastDocumentDescription == "Explicación");
        }
    }
}