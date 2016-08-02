using System;
using System.IO;
using Newtonsoft.Json;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Tests {
    internal class InsertableDocumentsBuilder {
        private string DocumentsPath { get; }

        public InsertableDocumentsBuilder(string documentsPath) {
            DocumentsPath = documentsPath;
        }

        public FileDocument ADocumentFor(string product, string version) {
            var documentToInsert = AnyDocumentFor(product: product, version: version);
            var jsonDocument = JsonConvert.SerializeObject(documentToInsert);
            var productPath = Path.Combine(DocumentsPath, documentToInsert.Id);
            File.WriteAllText(productPath, jsonDocument);
            return documentToInsert;
        }

        public void AnyOtherDocument() {
            ADocumentFor(AnyUniqueString(), AnyUniqueString());
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
}