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

        public void Save(Topic topic)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Topic topic)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<TopicToShow>> GetAllTopicsToShow()
        {
            var resultset = (await connection.Query<dynamic>(
                @"SELECT Topic.TopicId, Product.ProductName, Document.Title, Document.Description
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
                LastDocumentTitle = topic.Title,
                LastDocumentDescription = topic.Description,
            };
        }
    }
}