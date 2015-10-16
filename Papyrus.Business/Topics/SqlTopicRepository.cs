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

        public async Task<List<TopicToList>> GetAllTopicsToList()
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
            var topicsToShow = DistinctByTopicChoosingTheRowWithLatestDocumentAdded(resultset);
            return topicsToShow;
        }

        public async Task<EditableTopic> GetEditableTopicById(string topicId)
        {
            var product = (await connection.Query<DisplayableProduct>(@"SELECT Product.ProductId, Product.ProductName FROM Topic
                                                                        JOIN Product ON Topic.TopicId = @TopicId AND
                                                                                        Topic.ProductId = Product.ProductId",
                                                                        new { TopicId = topicId }))
                                                                        .FirstOrDefault();
            var versionRanges = (await connection
                .Query<dynamic>(@"SELECT VersionRangeId, FromVersionId, ToVersionId 
                                                    FROM VersionRange
                                                    WHERE TopicId = @TopicId", new { TopicId = topicId })).ToList();
            var observableVersionRanges = new ObservableCollection<EditableVersionRange>();
            foreach (var versionRange in versionRanges)
            {
                var documents = (await connection.Query<EditableDocument>(@"SELECT Title, Description, Content, Language
                                                                            FROM Document
                                                                            WHERE VersionRangeId = @VersionRangeId",
                                                                            new {VersionRangeId = versionRange.VersionRangeId}))
                                                                            .ToList();
                observableVersionRanges.Add(new EditableVersionRange(
                    fromVersionId: versionRange.FromVersionId,
                    toVersionId: versionRange.ToVersionId,
                    documents: documents
                ));
            }
            return new EditableTopic
            {
                TopicId = topicId,
                Product = product,
                VersionRanges = observableVersionRanges
            };
        }

        private static List<TopicToList> DistinctByTopicChoosingTheRowWithLatestDocumentAdded(IEnumerable<dynamic> dynamicTopics)
        {
            return dynamicTopics.GroupBy(topic => topic.TopicId)
                .Select(topics => topics.First())
                .Select(TopicToShowFromDynamic)
                .ToList();
        }

        public void Update(Topic topic)
        {
            throw new System.NotImplementedException();
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
            foreach (var documentLanguagePair in versionRange.Documents)
            {
                await InsertDocumentForVersionRange(documentLanguagePair, versionRange);
            }
        }

        private async Task InsertDocumentForVersionRange(LanguageDocumentPair languageDocumentPair, VersionRange versionRange)
        {
            await connection.Execute(@"INSERT INTO Document(DocumentId, Title, Description, Content, Language, VersionRangeId)
                                                    VALUES(@DocumentId, @Title, @Description, @Content, @Language, @VersionRangeId);",
                new
                {
                    DocumentId = languageDocumentPair.Document.DocumentId,
                    Title = languageDocumentPair.Document.Title,
                    Description = languageDocumentPair.Document.Description,
                    Content = languageDocumentPair.Document.Content,
                    Language = languageDocumentPair.Language,
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

        private static TopicToList TopicToShowFromDynamic(dynamic topic)
        {
            return new TopicToList
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