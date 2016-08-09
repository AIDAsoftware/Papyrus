using System;
using System.Linq;
using Papyrus.Business.Domain.Documents;
using Papyrus.Business.Domain.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Infrastructure.Repositories {
    public class FileDocumentsRepository : DocumentsRepository {
        // TODO : readonly field
        private readonly FileSystemProvider fileSystemProvider;

        public FileDocumentsRepository(FileSystemProvider fileSystemProvider) {
            this.fileSystemProvider = fileSystemProvider;
        }

        public Documentation GetDocumentationFor(VersionIdentifier versionId) {
            var documents = fileSystemProvider.GetAll<FileDocument>()
                .Where(d => d.ProductId == versionId.ProductId && d.VersionId == versionId.VersionId)
                .Select(d => new Document(d.Title, d.Description, d.Content, d.Language, new VersionIdentifier(d.ProductId, d.VersionId)))
                .ToList();
            return Documentation.WithDocuments(documents);
        }

        public void CreateDocumentFor(Document document) {
            var fileDocument = new FileDocument {
                Id = Guid.NewGuid().ToString(),
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language,
                ProductId = document.VersionIdentifier.ProductId,
                VersionId = document.VersionIdentifier.VersionId
            };
            fileSystemProvider.Persist(fileDocument);
        }
    }
}