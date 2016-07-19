using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.VersionRanges;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Business.Topics {
    public class SqlTopicQueryRepository : TopicQueryRepository {
        private readonly DatabaseConnection connection;

        public SqlTopicQueryRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task<List<TopicSummary>> GetAllTopicsSummariesFor(string language) {
            var resultset = (await connection.Query<dynamic>(
                @"SELECT Topic.TopicId, Product.ProductName, Product.ProductId, VersionRange.ToVersionId, Document.Title, Document.Description
                    FROM Topic
                    JOIN Product ON Product.ProductId = Topic.ProductId
                    JOIN VersionRange ON VersionRange.TopicId = Topic.TopicId
                    JOIN Document ON Document.VersionRangeId = VersionRange.VersionRangeId
                                        and Document.Language = @Language"
                , new{Language = language})).ToList();
            foreach (var topic in resultset) {
                await SetLastVersionDependingOnVersionRangeFor(topic);
            }
            var distinctedResulset = FilterSummariesForMostRecentVersion(resultset);
            return distinctedResulset.Select(TopicSummaryFromDynamic).ToList();
        }

        private async Task SetLastVersionDependingOnVersionRangeFor(dynamic topic) {
            if (topic.ToVersionId == LastProductVersion.Id) {
                topic.VersionName = LastProductVersion.Name;
                topic.Release = DateTime.MaxValue.ToString("yyyyMMdd");
            }
            else {
                var version = (await connection.Query<dynamic>(
                    @"SELECT VersionName, Release FROM ProductVersion WHERE VersionId = @VersionId"
                    , new {VersionId = topic.ToVersionId})).First();
                topic.VersionName = version.VersionName;
                topic.Release = version.Release.ToString("yyyyMMdd");
            }
        }

        public async Task<Topic> GetTopicById(string topicId)
        {
            var product = await GetRelatedProductFor(topicId);
            var versionRanges = await VersionRangesOf(topicId);
            var topic = new Topic(product.ProductId);
            versionRanges.ForEach(topic.AddVersionRange);
            return topic;
        }

        public async Task<List<ExportableDocument>> GetAllDocumentsFor(string product, string version, string language) {
            var documents = new List<ExportableDocument>();
            var dateOfWishedVersion = await GetReleaseFor(product, version);
            if (dateOfWishedVersion == default(DateTime)) return documents;
            var topicsForProduct = await SelectTopicsIdsFor(product);
            
            foreach (var topicId in topicsForProduct) {
                var versionRanges = await GetDynamicVersionRangesBy(topicId);
                foreach (var versionRange in versionRanges) {
                    var fromVersion = await SelectProductVersionById(versionRange.FromVersionId);
                    ProductVersion toVersion;
                    if (versionRange.ToVersionId == "*") {
                        toVersion = (await connection.Query<ProductVersion>(@"SELECT TOP 1 VersionId,                                     VersionName, Release
                                                            FROM ProductVersion
                                                            WHERE ProductId = @ProductId
                                                            ORDER BY Release DESC", 
                                                new { ProductId = product })).First();
                    }
                    else {
                        toVersion = await SelectProductVersionById(versionRange.ToVersionId); 
                    }
                    if (fromVersion.Release <= dateOfWishedVersion && 
                            dateOfWishedVersion <= toVersion.Release) {
                        var document = await GetTitleAndContentOfADocumentBy(language, versionRange);
                        if (!(document is NoDocument))
                            documents.Add(new ExportableDocument(document.Title, document.Content));
                    }
                }
            }
            return documents;
        }

        private async Task<DateTime> GetReleaseFor(string product, string version) {
            return (await connection.Query<DateTime>(@"SELECT Release From ProductVersion 
                                                        WHERE ProductId = @ProductId AND VersionName = @VersionName", 
                new {ProductId = product, VersionName = version})).FirstOrDefault();
        }

        private async Task<List<string>> SelectTopicsIdsFor(string product) {
            return (await connection.Query<string>(@"Select TopicId FROM Topic
                                                            WHERE Topic.ProductId = @ProductId", new { ProductId = product }))
                .ToList();
        }

        private async Task<List<dynamic>> GetDynamicVersionRangesBy(string topicId) {
            return (await connection.Query<dynamic>(@"SELECT VersionRangeId, FromVersionId, ToVersionId 
                                                            FROM VersionRange WHERE TopicId = @TopicId",
                new { TopicId = topicId })).ToList();
        }

        private async Task<dynamic> GetTitleAndContentOfADocumentBy(string language, dynamic versionRange) {
            var document = (await connection.Query<dynamic>(@"SELECT Title, Content FROM Document 
                                                                    WHERE VersionRangeId = @VersionRangeId
                                                                    AND Language = @Language",
                new { VersionRangeId = versionRange.VersionRangeId, Language = language }))
                .FirstOrDefault();
            if (document == null) return new NoDocument();
            return document;
        }

        private static List<dynamic> FilterSummariesForMostRecentVersion(IEnumerable<dynamic> dynamicTopics) {
            return dynamicTopics.OrderByDescending(t => t.Release)
                .GroupBy(topic => topic.TopicId)
                .Select(topics => topics.First())
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

        private async Task<List<VersionRange>> VersionRangesOf(string topicId)
        {
            var versionRanges = (await connection
                .Query<dynamic>(@"SELECT VersionRangeId, FromVersionId, ToVersionId 
                                FROM VersionRange WHERE TopicId = @TopicId",
                                new { TopicId = topicId })).ToList();
            foreach (var versionRange in versionRanges) {
                await AssignProductVersionTo(versionRange);
            }
            await AddDocumentsToEachVersionRangeIn(versionRanges);
            return versionRanges.Select(MapToVersionRange).ToList();
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

        private async Task AddDocumentsToEachVersionRangeIn(List<dynamic> versionRanges)
        {
            foreach (var versionRange in versionRanges)
            {
                versionRange.Documents = await DocumentsOf(versionRange);
            }
        }

        private VersionRange MapToVersionRange(dynamic versionRange)
        {
            var result = new VersionRange(versionRange.FromVersion, versionRange.ToVersion);
            var documents = (List<Document>) versionRange.Documents;
            documents.ForEach(result.AddDocument);
            return result;
        }

        private async Task<List<Document>> DocumentsOf(dynamic versionRange)
        {
            return (await connection.Query<Document>(@"SELECT Title, Description, Content, Language
                                                                            FROM Document
                                                                            WHERE VersionRangeId = @VersionRangeId",
                new { VersionRangeId = versionRange.VersionRangeId })).ToList();
        }

    }
}