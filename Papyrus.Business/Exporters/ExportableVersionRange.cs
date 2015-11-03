using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters
{
    public class ExportableVersionRange
    {
        public List<ProductVersion> Versions { get; private set; } 
        public List<ExportableDocument> Documents { get; private set; }

        public ExportableVersionRange()
        {
            Versions = new List<ProductVersion>();
            Documents = new List<ExportableDocument>();
        }

        public void AddDocument(ExportableDocument document)
        {
            Documents.Add(document);
        }

        public ExportableDocument GetDocumentByLanguage(string language)
        {
            return Documents.First(d => d.Language == language);
        }

        public void AddVersion(ProductVersion version)
        {
            Versions.Add(version);
        }

        public List<string> Languages()
        {
            return Documents.Select(d => d.Language).ToList();
        }

        public async Task ExportDocumentForProduct(ExportableProduct product, DirectoryInfo directory, string extension) {
            var productDirectory = directory.CreateSubdirectory(product.ProductName);
            await CreateDocumentsStructureForEachLanguageIn(productDirectory, extension);
        }

        private async Task CreateDocumentsStructureForEachLanguageIn(DirectoryInfo versionDirectory, string extension) {
            foreach (var language in Languages()) {
                await ConstructDocumentForLanguageInDirectory(language, versionDirectory, extension);
            }
        }

        private async Task ConstructDocumentForLanguageInDirectory(string language, DirectoryInfo versionDirectory, string extension)
        {
            var languageDirectory = versionDirectory.CreateSubdirectory(language);
            var docs = languageDirectory.CreateSubdirectory("docs");
            if (!File.Exists(Path.Combine(languageDirectory.FullName, "mkdocs.yml"))) {
                await WriteContentIn(Path.Combine(languageDirectory.FullName, "mkdocs.yml"), "site_name: SIMA Documentation");
                await WriteContentIn(Path.Combine(docs.FullName, "index.md"), "###Documentación de SIMA");                
            }
            var document = GetDocumentByLanguage(language);
            await document.ExportDocument(docs, extension);
        }

        private async Task WriteContentIn(string filePath, string content) {
            var encodedText = Encoding.UTF8.GetBytes(content);

            using (var sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true)) {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        public async Task ExportVersionRangeIn(DirectoryInfo directory, ExportableProduct product, string extension)
        {
            foreach (var productVersion in Versions) {
                var versionDirectory = directory.CreateSubdirectory(productVersion.VersionName);
                await ExportDocumentForProduct(product, versionDirectory, extension);
            }
        }

        public bool Contains(ProductVersion version) {
            return Versions.Contains(version);
        }

        public void AddDocuments(IEnumerable<ExportableDocument> documents) {
            Documents.AddRange(documents);
        }

        public void AddVersions(IEnumerable<ProductVersion> productVersions) {
            Versions.AddRange(productVersions);
        }
    }
}