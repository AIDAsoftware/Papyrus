using System.IO;
using System.Text;

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

        public virtual async Task<Document> GetDocumentByTopicId(string topicId)   //TODO: get Document By DocumentIdentity?
        {
            return await repository.GetDocument(topicId);
        }

        public virtual async Task Update(Document document)
        {
            ValidateDocumentToUpdate(document);

            if (await GetDocumentByTopicId(document.DocumentIdentity.TopicId) == null)
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

            if (!string.IsNullOrWhiteSpace(document.DocumentIdentity.TopicId))
                throw new DocumentIdMustNotBeDefinedException();
        }

        private static void ValidateDocumentToUpdate(Document document)
        {
            generalValidation(document);

            if (string.IsNullOrWhiteSpace(document.DocumentIdentity.TopicId))
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

        public async Task ExportDocumentsToFolder(DirectoryInfo targetDirectory) {
            if (!targetDirectory.Exists) targetDirectory.Create();
            var documents = await repository.GetAllDocuments();
            foreach (var document in documents) {
                await ExportDocument(document, targetDirectory);
            }
        }

        private async Task ExportDocument(Document document, DirectoryInfo targetDirectory) {
            var fileName = document.Title + ".md";
            var filePath = Path.Combine(targetDirectory.FullName, fileName);
            await WriteTextAsync(filePath, document.Content);
        }

        private async Task WriteTextAsync(string filePath, string text) {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true)) {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}