using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public interface TopicQueryRepository
    {
        Task<List<TopicSummary>> GetAllTopicsSummaries();
        Task<EditableTopic> GetEditableTopicById(string topicId);
        Task<List<ExportableTopic>> GetExportableTopicsForProduct(string productId);
        Task<List<ExportableTopic>>  GetExportableTopicsForProductVersion(string productId, ProductVersion version);
    }
}