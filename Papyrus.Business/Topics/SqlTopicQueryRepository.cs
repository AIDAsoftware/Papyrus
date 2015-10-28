using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Business.Topics
{
    public class SqlTopicQueryRepository : TopicQueryRepository
    {
        private readonly DatabaseConnection connection;

        public SqlTopicQueryRepository(DatabaseConnection connection)
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

        public Task<List<EditableTopic>> GetEditableTopicsForProduct(string productId)
        {
            throw new System.NotImplementedException();
        }

        private static List<TopicSummary> DistinctByTopicChoosingTheRowWithLatestDocumentAdded(IEnumerable<dynamic> dynamicTopics)
        {
            return dynamicTopics.GroupBy(topic => topic.TopicId)
                .Select(topics => topics.First())
                .Select(TopicSummaryFromDynamic)
                .ToList();
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

        private async Task<DisplayableProduct> GetRelatedProductFor(string topicId)
        {
            var product = (await connection.Query<DisplayableProduct>(@"SELECT Product.ProductId, Product.ProductName FROM Topic
                                                                        JOIN Product ON Topic.TopicId = @TopicId AND
                                                                                        Topic.ProductId = Product.ProductId",
                new { TopicId = topicId }))
                .FirstOrDefault();
            return product;
        }

        private async Task<ObservableCollection<EditableVersionRange>> GetVersionRangesOf(string topicId)
        {
            var versionRanges = (await connection
                .Query<dynamic>(@"SELECT VR.FromVersionId, ProductVersion.VersionName FromVersionName, ProductVersion.Release FromRelease,
                                        VR.ToVersionId, ToVersions.VersionName ToVersionName, ToVersions.Release ToRelease, VR.VersionRangeId 
                                    from VersionRange VR
                                    join ProductVersion on VR.FromVersionId = ProductVersion.VersionId
                                    join (
	                                    select VersionRangeId, ToVersionId, ProductVersion.VersionName, ProductVersion.Release
	                                    from VersionRange
	                                    join ProductVersion on ToVersionId = ProductVersion.VersionId) ToVersions
                                    on VR.ToVersionId = ToVersions.ToVersionId AND ToVersions.VersionRangeId = VR.VersionRangeId
                                    where VR.TopicId = @TopicId",
                                new { TopicId = topicId })).ToList();
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
                FromVersion = new ProductVersion(versionRange.FromVersionId, versionRange.FromVersionName, versionRange.FromRelease),
                ToVersion = new ProductVersion(versionRange.ToVersionId, versionRange.ToVersionName, versionRange.ToRelease),
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
                new { VersionRangeId = versionRange.VersionRangeId }))
                .ToList();
            return documents;
        }
    }
}