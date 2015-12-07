using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Business.Topics {
    public class SqlTopicQueryRepository : TopicQueryRepository {
        private readonly DatabaseConnection connection;

        public SqlTopicQueryRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task<List<TopicSummary>> GetAllTopicsSummaries() {
            var resultset = (await connection.Query<dynamic>(
                @"SELECT Topic.TopicId, Product.ProductName, Product.ProductId, VersionRange.ToVersionId, Document.Title, Document.Description
                    FROM Topic
                    JOIN Product ON Product.ProductId = Topic.ProductId
                    JOIN VersionRange ON VersionRange.TopicId = Topic.TopicId
                    JOIN Document ON Document.VersionRangeId = VersionRange.VersionRangeId"
                )).ToList();
            foreach (var topic in resultset) {
                if (topic.ToVersionId == LastProductVersion.Id) {
                    topic.VersionName = LastProductVersion.Name;
                }
                else {
                    topic.VersionName = (await connection.Query<string>(
                        @"SELECT VersionName, Release FROM ProductVersion WHERE VersionId = @VersionId"
                        , new { VersionId = topic.ToVersionId })).First();
                }
            }
            var orderedResultSet = resultset.OrderBy(t => t.Release);
            var topicsToShow = DistinctByTopicChoosingTheRowWithLatestDocumentAdded(orderedResultSet);
            return topicsToShow;
        }

        public async Task<EditableTopic> GetEditableTopicById(string topicId) {
            var product = await GetRelatedProductFor(topicId);
            var observableVersionRanges = await GetVersionRangesOf(topicId);
            return new EditableTopic {
                TopicId = topicId,
                Product = product,
                VersionRanges = observableVersionRanges
            };
        }

        public async Task<List<ExportableDocument>> GetAllDocumentsFor(string product, string version, string language, string documentRoute) {
            var documents = new List<ExportableDocument>();
            var dateOfWishedVersion = (await connection.Query<DateTime>(@"SELECT Release From ProductVersion 
                                                        WHERE ProductId = @ProductId AND VersionName = @VersionName", 
                                                        new {ProductId = product, VersionName = version})).First();
            var topicsForProduct = await SelectTopicsIdsFor(product);
            foreach (var topicId in topicsForProduct) {
                var versionRanges = await GetDynamicVersionRangeBy(topicId);
                foreach (var versionRange in versionRanges) {
                    var fromVersionRelease = await SelectProductVersionById(versionRange.FromVersionId);
                    var toVersionRelease = await SelectProductVersionById(versionRange.ToVersionId);
                    if (fromVersionRelease.Release <= dateOfWishedVersion && 
                            dateOfWishedVersion <= toVersionRelease.Release) {
                        var document = await GetTitleAndContentOfADocumentBy(language, versionRange);
                        documents.Add(new ExportableDocument(document.Title, document.Content, documentRoute));
                    }
                }
            }
            return documents;
        }

        private async Task<List<string>> SelectTopicsIdsFor(string product) {
            return (await connection.Query<string>(@"Select TopicId FROM Topic
                                                            WHERE Topic.ProductId = @ProductId", new { ProductId = product }))
                .ToList();
        }

        private async Task<List<dynamic>> GetDynamicVersionRangeBy(string topicId) {
            return (await connection.Query<dynamic>(@"SELECT VersionRangeId, FromVersionId, ToVersionId 
                                                            FROM VersionRange WHERE TopicId = @TopicId",
                new { TopicId = topicId })).ToList();
        }

        private async Task<dynamic> GetTitleAndContentOfADocumentBy(string language, dynamic versionRange) {
            return (await connection.Query<dynamic>(@"SELECT Title, Content FROM Document 
                                                                    WHERE VersionRangeId = @VersionRangeId
                                                                    AND Language = @Language",
                new { VersionRangeId = versionRange.VersionRangeId, Language = language }))
                .First();
        }

        private static List<TopicSummary> DistinctByTopicChoosingTheRowWithLatestDocumentAdded(IEnumerable<dynamic> dynamicTopics) {
            return dynamicTopics.GroupBy(topic => topic.TopicId)
                .Select(topics => topics.First())
                .Select(TopicSummaryFromDynamic)
                .ToList();
        }

        private static TopicSummary TopicSummaryFromDynamic(dynamic topic) {
            return new TopicSummary {
                TopicId = topic.TopicId,
                Product = new DisplayableProduct { ProductId = topic.ProductId, ProductName = topic.ProductName },
                VersionName = topic.VersionName,
                LastDocumentTitle = topic.Title,
                LastDocumentDescription = topic.Description,
            };
        }

        private async Task<DisplayableProduct> GetRelatedProductFor(string topicId) {
            var product = (await connection.Query<DisplayableProduct>(@"SELECT Product.ProductId, Product.ProductName FROM Topic
                                                                        JOIN Product ON Topic.TopicId = @TopicId AND
                                                                                        Topic.ProductId = Product.ProductId",
                new { TopicId = topicId }))
                .FirstOrDefault();
            return product;
        }

        private async Task<ObservableCollection<EditableVersionRange>> GetVersionRangesOf(string topicId) {
            var versionRanges = (await connection
                .Query<dynamic>(@"SELECT VersionRangeId, FromVersionId, ToVersionId 
                                FROM VersionRange WHERE TopicId = @TopicId",
                                new { TopicId = topicId })).ToList();
            foreach (var versionRange in versionRanges) {
                await AssignProductVersionTo(versionRange);
            }
            await AddDocumentsForEachVersionRangeIn(versionRanges);
            var editableVersionRanges = versionRanges.Select(MapToEditableVersionRange);
            return new ObservableCollection<EditableVersionRange>(editableVersionRanges);
        }

        private async Task AssignProductVersionTo(dynamic versionRange) {
            string fromVersionId = versionRange.FromVersionId;
            versionRange.FromVersion = await SelectProductVersionById(fromVersionId);
            if (versionRange.ToVersionId == LastProductVersion.Id) {
                versionRange.ToVersion = new LastProductVersion();
                return;
            }
            var toVersionId = versionRange.ToVersionId;
            versionRange.ToVersion = await SelectProductVersionById(toVersionId);
        }

        private async Task<ProductVersion> SelectProductVersionById(string id) {
            return (await connection
                .Query<ProductVersion>(@"SELECT VersionId, VersionName, Release FROM ProductVersion WHERE VersionId = @VersionId",
                    new {VersionId = id})).First();
        }

        private async Task AddDocumentsForEachVersionRangeIn(IEnumerable<dynamic> versionRanges) {
            foreach (var versionRange in versionRanges) {
                versionRange.Documents = await GetDocumentsOf(versionRange);
            }
        }

        private EditableVersionRange MapToEditableVersionRange(dynamic versionRange) {
            var fromVersion = versionRange.FromVersion;
            var toVersion = versionRange.ToVersion;
            var editableVersionRange = new EditableVersionRange() {
                FromVersion = versionRange.FromVersion,
                ToVersion = versionRange.ToVersion
            };

            foreach (var editableDocument in versionRange.Documents) {
                editableVersionRange.Documents.Add(editableDocument);
            }
            return editableVersionRange;
        }

        private async Task<List<EditableDocument>> GetDocumentsOf(dynamic versionRange) {
            var documents = (await connection.Query<EditableDocument>(@"SELECT Title, Description, Content, Language
                                                                            FROM Document
                                                                            WHERE VersionRangeId = @VersionRangeId",
                new { VersionRangeId = versionRange.VersionRangeId }))
                .ToList();
            return documents;
        }
    }
}