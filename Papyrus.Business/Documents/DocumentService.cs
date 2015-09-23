namespace Papyrus.Business.Documents
{
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
            ValidateDocumentToCreation(document);
            document.GenerateAutomaticId();
            await repository.Save(document);
        }

        public virtual async Task<Document> GetDocumentById(string id)   //TODO: get Document By DocumentIdentity?
        {
            return await repository.GetDocument(id);
        }

        public virtual async Task Update(Document document)
        {
            ValidateDocumentToUpdate(document);

            if (await GetDocumentById(document.DocumentIdentity.Id) == null)
                throw new DocumentNotFoundException();
            await repository.Update(document);
        }

        public virtual async Task<Document[]> AllDocuments()
        {
            return (await repository.GetAllDocuments()).ToArray();
        }

        private static void ValidateDocumentToCreation(Document document)
        {
            generalValidation(document);

            if (!string.IsNullOrWhiteSpace(document.DocumentIdentity.Id))
                throw new DocumentIdMustNotBeDefinedException();
        }

        private static void ValidateDocumentToUpdate(Document document)
        {
            generalValidation(document);

            if (string.IsNullOrWhiteSpace(document.DocumentIdentity.Id))
                throw new DocumentIdMustBeDefinedException();
        }

        private static void generalValidation(Document document)
        {
            if (string.IsNullOrWhiteSpace(document.DocumentIdentity.ProductId))
                throw new DocumentMustBeAssignedToAProductException();

            if (string.IsNullOrWhiteSpace(document.DocumentIdentity.VersionId))
                throw new DocumentMustBeAssignedToAProductVersionException();

            if (string.IsNullOrWhiteSpace(document.DocumentIdentity.Language))
                throw new DocumentMustHaveALanguageException();
        }
    }
}