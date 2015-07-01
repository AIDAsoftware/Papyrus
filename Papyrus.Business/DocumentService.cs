namespace Papyrus.Business
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Tests;

    public class DocumentService
    {
        private readonly DocumentRepository repository;

        public DocumentService(DocumentRepository repository)
        {
            this.repository = repository;
        }

        public void Create(Document document)
        {
            if (!String.IsNullOrWhiteSpace(document.Id))
                throw new DocumentIdCouldNotBeDefinedException();
            document.GenerateAutomaticId();
            repository.Save(document);
        }

        public async Task<Document> GetDocumentById(string id)
        {
            return await repository.GetDocument(id);
        }

        public async Task Update(Document document)
        {
            if (String.IsNullOrWhiteSpace(document.Id))
                throw new DocumentIdCouldBeDefinedException();
            await repository.Update(document);
        }

        public void Remove(string documentId)
        {
            repository.Delete(documentId);
        }

        public Document[] AllDocuments()
        {
            return repository.GetAllDocuments().ToArray();
        }
    }
}