namespace Papyrus.Business
{
    public interface DocumentRepository
    {
        void Save(Document document);
        Document GetDocument(string id);
        void Update(Document document);
        void Delete(Document document);
    }
}