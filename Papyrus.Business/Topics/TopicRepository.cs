using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papyrus.Business.Topics
{
    public interface CommandTopicRepository
    {
        Task Save(Topic topic);
        Task Update(Topic topic);
        Task Delete(Topic topic);
    }

    public interface QueryTopicRepository
    {
        Task<List<TopicSummary>> GetAllTopicsSummaries();
        Task<EditableTopic> GetEditableTopicById(string topicId);
        Task<List<EditableTopic>> GetEditableTopicsForProduct(string productId);
    }
}