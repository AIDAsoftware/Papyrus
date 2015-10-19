using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public interface TopicRepository
    {
        Task Save(Topic topic);
        Task Update(Topic topic);
        Task<List<TopicSummary>> GetAllTopicsSummaries();
        Task<EditableTopic> GetEditableTopicById(string topicId);
    }
}