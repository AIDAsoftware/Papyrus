using System.Threading.Tasks;

namespace Papyrus.Business.Topics
{
    public interface TopicCommandRepository
    {
        Task Save(Topic topic);
        Task Update(Topic topic);
        Task Delete(Topic topic);
    }
}