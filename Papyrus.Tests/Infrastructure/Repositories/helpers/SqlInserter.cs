using System.Threading.Tasks;
using Papyrus.Business.Topics;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories.TopicRepository
{
    public class SqlInserter
    {
        private DatabaseConnection dbConnection;

        public SqlInserter(DatabaseConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task Insert(Topic topic)
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
            await dbConnection.Execute(
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