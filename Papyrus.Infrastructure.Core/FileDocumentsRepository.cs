using System;
using System.Linq;
using Papyrus.Business;

namespace Papyrus.Infrastructure.Core {
    public class FileDocumentsRepository : DocumentsRepository {
        private FileRepository FileRepository { get; }

        public FileDocumentsRepository(FileRepository fileRepository) {
            FileRepository = fileRepository;
        }

        public Documentation GetDocumentationFor(string productId, string versionId) {
            var documents = FileRepository.GetAll<FileDocument>()
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
            FileRepository.Create(fileDocument);
        }
    }
}