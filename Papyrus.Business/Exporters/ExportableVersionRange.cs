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
            var productDirectory = product.ExportIn(directory);
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
            await CreateMkDocsProjectIfNeeded(languageDirectory, docs);
            var document = GetDocumentByLanguage(language);
            await document.ExportDocument(docs, extension);
        }

        //TODO: this is not ExportableVersionRange Responsibility
        private static async Task CreateMkDocsProjectIfNeeded(DirectoryInfo baseDirectory, DirectoryInfo docs) {
            var ymlDocumentPath = Path.Combine(baseDirectory.FullName, "mkdocs.yml");
            if (!File.Exists(ymlDocumentPath)) {
                await FileWriter.WriteFileWithContent(ymlDocumentPath, "site_name: SIMA Documentation");
                await FileWriter.WriteFileWithContent(Path.Combine(docs.FullName, "index.md"), "###Documentación de SIMA");
            }
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