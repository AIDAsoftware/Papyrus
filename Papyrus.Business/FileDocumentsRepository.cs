using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Papyrus.Business {
    public class FileDocumentsRepository : DocumentsRepository {
        private string DocumentsPath { get; }

        public FileDocumentsRepository(string documentsPath) {
            DocumentsPath = documentsPath;
        }

        public Documentation GetDocumentationFor(string productId, string versionId) {
            var directory = new DirectoryInfo(DocumentsPath);
            var files = directory.GetFiles();
            var documents = files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<FileDocument>)
                .Where(d => d.ProductId == productId && d.VersionId == versionId)
                .Select(d => new Document(d.Title, d.Description, d.Content, d.Language))
                .ToList();
            return Documentation.WithDocuments(documents);
        }

        public void CreateDocumentFor(Document document, string productId, string versionId) {
            Directory.CreateDirectory(DocumentsPath);
            var fileDocument = new FileDocument {
                Id = Guid.NewGuid().ToString(),
                Title = document.Title,
                Description = document.Description,
                Content = document.Content,
                Language = document.Language,
                ProductId = productId,
                VersionId = versionId
            };
            var documentPath = Path.Combine(DocumentsPath, fileDocument.Id);
            var jsonDocument = JsonConvert.SerializeObject(fileDocument);
            File.WriteAllText(documentPath, jsonDocument);
        }
    }
}