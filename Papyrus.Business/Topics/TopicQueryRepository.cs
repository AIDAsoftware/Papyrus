using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Topics
{
    public interface TopicQueryRepository
    {
        Task<List<TopicSummary>> GetAllTopicsSummaries();
        Task<EditableTopic> GetEditableTopicById(string topicId);
        Task<List<ExportableTopic>> GetEditableTopicsForProduct(string productId);
    }
}