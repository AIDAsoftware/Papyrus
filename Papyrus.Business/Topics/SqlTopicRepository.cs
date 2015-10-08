using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Business.Topics
{
    public class SqlTopicRepository : TopicRepository
    {
        private readonly DatabaseConnection connection;

        public SqlTopicRepository(DatabaseConnection connection)
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
            foreach(var versionRange in topic.VersionRanges)
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
                foreach (KeyValuePair<string, Document2> document in versionRange.documents)
                {
                    await connection.Execute(@"INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
                                                    VALUES(@DocumentId, @Title, @Description, @Content, @Language, @VersionRangeId);",
                                                    new
                                                    {
                                                        DocumentId = document.Value.DocumentId,
                                                        Title = document.Value.Title,
                                                        Description = document.Value.Description,
                                                        Content = document.Value.Content,
                                                        Language = document.Key,
                                                        VersionRangeId = versionRange.VersionRangeId    
                                                    });
                }
            };

        }

        public void Update(Topic topic)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<TopicToShow>> GetAllTopicsToShow()
        {
            var resultset = (await connection.Query<dynamic>(
                @"SELECT Topic.TopicId, Product.ProductName, ProductVersion.VersionName, Document.Title, Document.Description
                    FROM Topic
                    JOIN Product ON Product.ProductId = Topic.ProductId
                    JOIN VersionRange ON VersionRange.TopicId = Topic.TopicId
                    JOIN ProductVersion ON VersionRange.ToVersionId = ProductVersion.VersionId
                    JOIN Document ON Document.VersionRangeId = VersionRange.VersionRangeId
                    ORDER BY ProductVersion.Release DESC"
                ));
            var topicsToShow = resultset.GroupBy(topic => topic.TopicId)
                                        .Select(topics => topics.First())
                                        .Select(TopicToShowFromDynamic)
                                        .ToList();
            return topicsToShow;
        }

        private static TopicToShow TopicToShowFromDynamic(dynamic topic)
        {
            return new TopicToShow
            {
                TopicId = topic.TopicId,
                ProductName = topic.ProductName,
                VersionName = topic.VersionName,
                LastDocumentTitle = topic.Title,
                LastDocumentDescription = topic.Description,
            };
        }
    }
}