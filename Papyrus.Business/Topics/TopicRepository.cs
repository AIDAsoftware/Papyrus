using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public interface TopicRepository
    {
        Task Save(Topic topic);
        void Update(Topic topic);
        Task<List<TopicToList>> GetAllTopicsToList();
        Task<EditableTopic> GetEditableTopicById(string topicId);
    }
}