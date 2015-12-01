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
        Task<List<ExportableDocument>> GetAllDocumentsFor(Product product, string version, string language, string documentRoute);
    }
}