using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
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

        public async Task<List<TopicSummary>> GetAllTopicsSummaries()
        {
            var resultset = (await connection.Query<dynamic>(
                @"SELECT Topic.TopicId, Product.ProductName, Product.ProductId, ProductVersion.VersionName, Document.Title, Document.Description
                    FROM Topic
                    JOIN Product ON Product.ProductId = Topic.ProductId
                    JOIN VersionRange ON VersionRange.TopicId = Topic.TopicId
                    JOIN ProductVersion ON VersionRange.ToVersionId = ProductVersion.VersionId
                    JOIN Document ON Document.VersionRangeId = VersionRange.VersionRangeId
                    ORDER BY ProductVersion.Release DESC"
                ));
            var topicsToShow = DistinctByTopicChoosingTheRowWithLatestDocumentAdded(resultset);
            return topicsToShow;
        }

        public async Task<EditableTopic> GetEditableTopicById(string topicId)
        {
            var product = await GetRelatedProductFor(topicId);
            var observableVersionRanges = await GetVersionRangesOf(topicId);
            return new EditableTopic
            {
                TopicId = topicId,
                Product = product,
                VersionRanges = observableVersionRanges
            };
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
            var versionRangeIds = await connection.Query<string>(@"SELECT VersionRangeId FROM VersionRange
                                                                    WHERE TopicId = @TopicId",
                                                                    new {TopicId = topic.TopicId});
            await DeleteVersionRangesOf(topic);
            await DeleteDocumentsForEachVersionRangeIn(versionRangeIds);
            await InsertVersionRangesOf(topic);
        }

        private async Task DeleteDocumentsForEachVersionRangeIn(IEnumerable<string> versionRangeIds)
        {
            foreach (var versionRangeId in versionRangeIds)
            {
                await connection.Execute(@"DELETE FROM Document WHERE VersionRangeId = @VersionRangeId",
                    new {VersionRangeId = versionRangeId});
            }
        }

        private async Task DeleteVersionRangesOf(Topic topic)
        {
            await connection.Execute(@"DELETE FROM VersionRange WHERE TopicId = @TopicId",
                new {TopicId = topic.TopicId});
        }

        private static List<TopicSummary> DistinctByTopicChoosingTheRowWithLatestDocumentAdded(IEnumerable<dynamic> dynamicTopics)
        {
            return dynamicTopics.GroupBy(topic => topic.TopicId)
                .Select(topics => topics.First())
                .Select(TopicSummaryFromDynamic)
                .ToList();
        }

        private async Task<DisplayableProduct> GetRelatedProductFor(string topicId)
        {
            var product = (await connection.Query<DisplayableProduct>(@"SELECT Product.ProductId, Product.ProductName FROM Topic
                                                                        JOIN Product ON Topic.TopicId = @TopicId AND
                                                                                        Topic.ProductId = Product.ProductId",
                new {TopicId = topicId}))
                .FirstOrDefault();
            return product;
        }

        private async Task<ObservableCollection<EditableVersionRange>> GetVersionRangesOf(string topicId)
        {
            var versionRanges = (await connection
                .Query<dynamic>(@"SELECT VersionRangeId, FromVersionId, ToVersionId 
                                                    FROM VersionRange
                                                    WHERE TopicId = @TopicId", new {TopicId = topicId})).ToList();
            await AddDocumentsForEachVersionRangeIn(versionRanges);
            var editableVersionRanges = versionRanges.Select(MapToEditableVersionRange);
            return new ObservableCollection<EditableVersionRange>(editableVersionRanges);
        }

        private async Task AddDocumentsForEachVersionRangeIn(IEnumerable<dynamic> versionRanges)
        {
            foreach (var versionRange in versionRanges)
            {
                versionRange.Documents = await GetDocumentsOf(versionRange);
            }
        }

        private EditableVersionRange MapToEditableVersionRange(dynamic versionRange)
        {
            var editableVersionRange = new EditableVersionRange()
            {
                FromVersionId = versionRange.FromVersionId,
                ToVersionId = versionRange.ToVersionId,
            };

            foreach (var editableDocument in versionRange.Documents)
            {
                editableVersionRange.Documents.Add(editableDocument);
            }
            return editableVersionRange;
        }

        private async Task<List<EditableDocument>> GetDocumentsOf(dynamic versionRange)
        {
            var documents = (await connection.Query<EditableDocument>(@"SELECT Title, Description, Content, Language
                                                                            FROM Document
                                                                            WHERE VersionRangeId = @VersionRangeId",
                new {VersionRangeId = versionRange.VersionRangeId}))
                .ToList();
            return documents;
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

        private static TopicSummary TopicSummaryFromDynamic(dynamic topic)
        {
            return new TopicSummary
            {
                TopicId = topic.TopicId,
                Product = new DisplayableProduct { ProductId = topic.ProductId, ProductName = topic.ProductName },
                VersionName = topic.VersionName,
                LastDocumentTitle = topic.Title,
                LastDocumentDescription = topic.Description,
            };
        }
    }
}