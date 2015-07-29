namespace Papyrus.Business.Documents
{
    using System;
    using System.Threading.Tasks;
    using Exceptions;

    public class DocumentService
    {
        private readonly DocumentRepository repository;

        public DocumentService(DocumentRepository repository)
        {
            this.repository = repository;
        }

        public virtual async Task Create(Document document)
        {
            if (!String.IsNullOrWhiteSpace(document.Id))
                throw new DocumentIdCouldNotBeDefinedException();
            document.GenerateAutomaticId();
            await repository.Save(document);
        }

        public virtual async Task<Document> GetDocumentById(string id)
        {
            return await repository.GetDocument(id);
        }

        public async Task Update(Document document)
        {
            if (String.IsNullOrWhiteSpace(document.Id))
                throw new DocumentIdCouldBeDefinedException();
            await repository.Update(document);
        }

        public async Task Remove(string documentId)
        {
            await repository.Delete(documentId);
        }

        public virtual async Task<Document[]> AllDocuments()
        {
            return (await repository.GetAllDocuments()).ToArray();
        }
    }
}