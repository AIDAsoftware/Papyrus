namespace Papyrus.Business.Documents
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface DocumentRepository
    {
        Task Save(Document document);
        Task<Document> GetDocument(string topicId);
        Task Update(Document document);
        Task Delete(string topicId);
        Task<List<Document>> GetAllDocuments();
    }
}