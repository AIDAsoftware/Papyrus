namespace Papyrus.Business
{
    public class DocumentService
    {
        private readonly DocumentRepository repository;

        public DocumentService(DocumentRepository repository)
        {
            this.repository = repository;
        }

        public void Create(Document document)
        {
            repository.Save(document);
        }

        public Document GetDocumentById(string id)
        {
            return repository.GetDocument(id);
        }

        public void Update(Document document)
        {
            repository.Update(document);
        }

        public void Remove(Document document)
        {
            repository.Delete(document);
        }
    }
}