using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.VersionRanges;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Business.Topics
{
    public class SqlTopicCommandRepository : TopicCommandRepository
    {
        private readonly DatabaseConnection connection;

        public SqlTopicCommandRepository(DatabaseConnection connection)
        {
            this.connection = connection;
        }

        public async Task Save(Topic topic)
        {
            await connection.Execute("INSERT INTO Topic(TopicId, ProductId) VALUES (@TopicId, @ProductId)",
                new
                {
                    TopicId = topic.TopicId,
                    ProductId = topic.ProductId
                });
            await InsertVersionRangesOf(topic);
        }

        public async Task Update(Topic topic)
        {
            await DeleteVersionRangesOf(topic.TopicId);
            await InsertVersionRangesOf(topic);
        }

        public async Task Delete(string topicId)
        {
            await connection.Execute(@"DELETE FROM Topic WHERE TopicId=@TopicId", new { TopicId = topicId });
            await DeleteVersionRangesOf(topicId);
        }

        private async Task InsertVersionRangesOf(Topic topic)
        {
            foreach (var versionRange in topic.VersionRanges)
            {
                await InsertVersionRangeForTopic(versionRange, topic);
                await InsertDocumentsOf(versionRange);
            }
        }

        private async Task InsertDocumentsOf(VersionRange versionRange)
        {
            foreach (var document in versionRange.Documents)
            {
                await InsertDocumentForVersionRange(document, versionRange);
            }
        }

        private async Task InsertDocumentForVersionRange(Document document, VersionRange versionRange)
        {
            await connection.Execute(@"INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
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

        private async Task InsertVersionRangeForTopic(VersionRange versionRange, Topic topic)
        {
            await connection.Execute(@"INSERT INTO VersionRange(VersionRangeId, FromVersionId, ToVersionId, TopicId)
                                                VALUES (@VersionRangeId, @FromVersionId, @ToVersionId, @TopicId)",
                new
                {
                    VersionRangeId = versionRange.VersionRangeId,
                    FromVersionId = versionRange.FromVersionId,
                    ToVersionId = versionRange.ToVersionId,
                    TopicId = topic.TopicId
                });
        }

        private async Task DeleteVersionRangesOf(string topicId)
        {
            var versionRangeIds = await connection.Query<string>(@"SELECT VersionRangeId FROM VersionRange
                                                                    WHERE TopicId = @TopicId",
                                                                    new { TopicId = topicId });
            await connection.Execute(@"DELETE FROM VersionRange WHERE TopicId = @TopicId",
                new { TopicId = topicId });
            await DeleteDocumentsForEachVersionRangeIn(versionRangeIds);
        }

        private async Task DeleteDocumentsForEachVersionRangeIn(IEnumerable<string> versionRangeIds)
        {
            foreach (var versionRangeId in versionRangeIds)
            {
                await connection.Execute(@"DELETE FROM Document WHERE VersionRangeId = @VersionRangeId",
                    new { VersionRangeId = versionRangeId });
            }
        }
    }
}