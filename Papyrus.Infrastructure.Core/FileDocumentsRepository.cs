using System;
using System.Linq;
using Papyrus.Business;

namespace Papyrus.Infrastructure.Core {
    public class FileDocumentsRepository : DocumentsRepository {
        // TODO : readonly field
        private FileSystemProvider FileSystemProvider { get; }

        public FileDocumentsRepository(FileSystemProvider fileSystemProvider) {
            FileSystemProvider = fileSystemProvider;
        }

        public Documentation GetDocumentationFor(string productId, string versionId) {
            var documents = FileSystemProvider.GetAll<FileDocument>()
                .Where(d => d.ProductId == productId && d.VersionId == versionId)
                .Select(d => new Document(d.Title, d.Description, d.Content, d.Language))
                .ToList();
            return Documentation.WithDocuments(documents);
        }

        public void CreateDocumentFor(Document document, string productId, string versionId) {
            var fileDocument = new FileDocument {
                Id = Guid.NewGuid().ToString(),
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language,
                ProductId = productId,
                VersionId = versionId
            };
            FileSystemProvider.Persist(fileDocument);
        }
    }
}