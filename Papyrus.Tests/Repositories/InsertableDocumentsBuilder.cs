using System;
using System.IO;
using Newtonsoft.Json;
using Papyrus.Infrastructure.Repositories;

namespace Papyrus.Tests.Repositories {
    internal class InsertableDocumentsBuilder {
        private string DocumentsPath { get; }

        public InsertableDocumentsBuilder(string documentsPath) {
            DocumentsPath = documentsPath;
        }

        public SerializableDocument ADocumentFor(string product, string version) {
            var documentToInsert = AnyDocumentFor(product: product, version: version);
            var jsonDocument = JsonConvert.SerializeObject(documentToInsert);
            var productPath = Path.Combine(DocumentsPath, documentToInsert.Id);
            File.WriteAllText(productPath, jsonDocument);
            return documentToInsert;
        }

        public void AnyOtherDocument() {
            ADocumentFor(AnyUniqueString(), AnyUniqueString());
        }

        private static SerializableDocument AnyDocumentFor(string product, string version) {
            return new SerializableDocument {
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
}