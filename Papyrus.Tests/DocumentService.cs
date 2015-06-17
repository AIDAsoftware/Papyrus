namespace Papyrus.Tests
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
    }
}