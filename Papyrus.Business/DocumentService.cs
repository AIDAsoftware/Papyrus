namespace Papyrus.Business
{
    using System;
    using Tests;
    using static System.String;

    public class DocumentService
    {
        private readonly DocumentRepository repository;

        public DocumentService(DocumentRepository repository)
        {
            this.repository = repository;
        }

        public void Create(Document document)
        {
            if (!IsNullOrWhiteSpace(document.Id))
                throw new DocumentIdCouldNotBeDefinedException();
            document.GenerateAutomaticId();
            repository.Save(document);
        }

        public Document GetDocumentById(string id)
        {
            return repository.GetDocument(id);
        }

        public void Update(Document document)
        {
            if(IsNullOrWhiteSpace(document.Id))
                throw new DocumentIdCouldBeDefinedException();
            repository.Update(document);
        }

        public void Remove(string documentId)
        {
            repository.Delete(documentId);
        }
    }
}