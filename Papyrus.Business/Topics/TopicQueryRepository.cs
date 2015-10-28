using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papyrus.Business.Topics
{
    public interface TopicQueryRepository
    {
        Task<List<TopicSummary>> GetAllTopicsSummaries();
        Task<EditableTopic> GetEditableTopicById(string topicId);
        Task<List<EditableTopic>> GetEditableTopicsForProduct(string productId);
    }
}