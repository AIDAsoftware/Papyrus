using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Papyrus.Business;

namespace Papyrus.Tests {
    [TestFixture]
    public class FileDocumentsRepositoryShould {
        public readonly string DocumentsPath =
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Documents"));

        [SetUp]
        public void Setup() {
            Directory.CreateDirectory(DocumentsPath);
        }

        [TearDown]
        public void TearDown() {
            Directory.Delete(DocumentsPath, true);
        }

        [Test]
        public void get_documents_for_a_concrete_version() {
            var productId = AnyUniqueString();
            var versionId = AnyUniqueString();
            var documentToInsert = AnyDocumentFor(product: productId, version: versionId);
            var jsonDocument = JsonConvert.SerializeObject(documentToInsert);
            var productPath = Path.Combine(DocumentsPath, documentToInsert.Id);
            File.WriteAllText(productPath, jsonDocument);

            var documentsRepository = new FileDocumentsRepository(DocumentsPath);
            var documents = documentsRepository.GetDocumentationFor(productId, versionId);

            documents.ToList().Should().HaveCount(1);
            var retrievedDocument = documents.ToList().First();
            retrievedDocument.Title.Should().Be(documentToInsert.Title);
            retrievedDocument.Content.Should().Be(documentToInsert.Content);
            retrievedDocument.Description.Should().Be(documentToInsert.Description);
            retrievedDocument.Language.Should().Be(documentToInsert.Language);
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
        private string Path { get; }

        public FileDocumentsRepository(string path) {
            Path = path;
        }

        public Documentation GetDocumentationFor(string productId, string versionId) {
            var directory = new DirectoryInfo(Path);
            var files = directory.GetFiles();
            var documents = files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<FileDocument>)
                .Select(d => new Document(d.Title, d.Description, d.Content, d.Language))
                .ToList();
            return Documentation.WithDocuments(documents);
        }

        public void CreateDocumentFor(Document document, string productId, string versionId) {
            throw new NotImplementedException();
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