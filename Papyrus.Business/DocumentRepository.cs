namespace Papyrus.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface DocumentRepository
    {
        Task Save(Document document);
        Task<Document> GetDocument(string id);
        Task Update(Document document);
        Task Delete(string documentId);
        IEnumerable<Document> GetAllDocuments();
    }
}