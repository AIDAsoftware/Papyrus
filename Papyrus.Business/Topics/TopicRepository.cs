using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papyrus.Business.Topics
{
    public interface TopicRepository
    {
        Task Save(Topic topic);
        void Update(Topic topic);
        Task<List<TopicToShow>> GetAllTopicsToShow();
    }
}