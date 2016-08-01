using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using Papyrus.Business;

namespace Papyrus.Tests {
    [TestFixture]
    public class FileDocumentsRepositoryShould {
        public readonly string DocumentsPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Documents"));

        private FileDocumentsRepository documentsRepository;

        [SetUp]
        public void Setup() {
            Directory.CreateDirectory(DocumentsPath);
            documentsRepository = new FileDocumentsRepository(DocumentsPath);
        }

        [TearDown]
        public void TearDown() {
            Directory.Delete(DocumentsPath, true);
        }

        [Test]
        public void get_only_documents_for_a_concrete_version() {
            var expectedDocument = AnyUniquePersistedDocument();
            AnyUniquePersistedDocument();

            var documents = GetDocumentationFor(expectedDocument.ProductId, expectedDocument.VersionId);

            documents.Should().HaveCount(1);
            documents.Should().Contain(ADocumentEquivalentTo(expectedDocument));
        }

        [Test]
        public void create_a_document_for_a_concrete_version() {
            var productId = AnyUniqueString();
            var versionId = AnyUniqueString();
            var documentToInsert = AnyDocument();

            documentsRepository.CreateDocumentFor(documentToInsert, productId, versionId);
            
            var documents = GetAllDocumentsFrom(DocumentsPath);
            documents.Should().HaveCount(1);
            documents.First().ShouldBeEquivalentTo(documentToInsert, 
                options => options.Excluding(d => d.ProductId)
                                .Excluding(d => d.VersionId)
                                .Excluding(d => d.Id));
            documents.First().VersionId.Should().Be(versionId);
            documents.First().ProductId.Should().Be(productId);
        }

        private List<FileDocument> GetAllDocumentsFrom(string documentsPath) {
            return new DirectoryInfo(documentsPath)
                .GetFiles()
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<FileDocument>)
                .ToList();
        }

        private List<Document> GetDocumentationFor(string product, string version) {
            var documents =
                documentsRepository.GetDocumentationFor(product, version).ToList();
            return documents;
        }

        private FileDocument AnyUniquePersistedDocument() {
            var documentToInsert = AnyDocumentFor(product: AnyUniqueString(), version: AnyUniqueString());
            var jsonDocument = JsonConvert.SerializeObject(documentToInsert);
            var productPath = Path.Combine(DocumentsPath, documentToInsert.Id);
            File.WriteAllText(productPath, jsonDocument);
            return documentToInsert;
        }

        private static Document AnyDocument() {
            return new Document(AnyUniqueString(), AnyUniqueString(), AnyUniqueString(), AnyUniqueString());
        }

        private static Expression<Func<Document, bool>> ADocumentEquivalentTo(FileDocument documentToInsert) {
            return document => 
                document.Title == documentToInsert.Title && 
                document.Description == documentToInsert.Description && 
                document.Content == documentToInsert.Content && 
                document.Language == documentToInsert.Language;
        }

        private static FileDocument AnyDocumentFor(string product, string version) {
            return new FileDocument {
                Id = AnyUniqueString(),
                Title = AnyUniqueString(),
                Content = AnyUniqueString(),
                Description = AnyUniqueString(),
                Language = AnyUniqueString(),
                ProductId = product,
                VersionId = version
            };
        }

        private static string AnyUniqueString() {
            return Guid.NewGuid().ToString();
        }
    }

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

    public class FileDocument {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ProductId { get; set; }
        public string VersionId { get; set; }
    }
}