using System.Collections.Generic;
using System.Threading.Tasks;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Topics
{
    public interface TopicQueryRepository
    {
        Task<List<TopicSummary>> GetAllTopicsSummariesFor(string language);
        Task<Topic> GetTopicById(string topicId);
        Task<List<ExportableDocument>> GetAllDocumentsFor(string product, string version, string language);
    }
}