namespace Papyrus.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface DocumentRepository
    {
        void Save(Document document);
        Document GetDocument(string id);
        Task Update(Document document);
        void Delete(string documentId);
        IEnumerable<Document> GetAllDocuments();
    }
}