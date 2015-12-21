using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class MkdocsExporter {
        private const string YmlFileName = "mkdocs.yml";
        private const string MarkDownExtension = ".md";
        private readonly string mkdocsTheme = "theme: readthedocs";
        private readonly string siteName = "site_name: SIMA Documentation";
        private static readonly string NewLine = System.Environment.NewLine;
        public const string IndexContent = "SIMA Documentation";
        private const string Tab = "\t";

        public MkdocsExporter() {
        }

        public virtual async Task Export(WebSite webSite, string path) {
            var docsPath = Path.Combine(path, "docs");
            var docsDirectory = Directory.CreateDirectory(docsPath);
            await WriteFileIn(Path.Combine(docsDirectory.FullName, "index.md"), IndexContent);
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, docsDirectory);
                await WriteYmlFileIn(path, document);
            }
        }

        private async Task WriteYmlFileIn(string path, ExportableDocument document) {
            var ymlPath = Path.Combine(path, YmlFileName);
            if (!File.Exists(ymlPath)) {
                await WriteFileIn(ymlPath, mkdocsTheme);
                await WriteFileIn(ymlPath, siteName);
                await WriteFileIn(ymlPath, "pages:");                
            }
            await WriteFileIn(ymlPath, MkdocsPagePresentationFor(document));
        }

        private static string MkdocsPagePresentationFor(ExportableDocument document) {
            return Tab + document.ExportableTitle + ": " + document.Title;
        }

        private static async Task ExportDocumentIn(ExportableDocument document, DirectoryInfo directory) {
            var documentDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, document.Route));
            var documentPath = Path.Combine(documentDirectory.FullName, document.ExportableTitle + MarkDownExtension);
            await WriteFileIn(documentPath, document.Content);
        }

        private static async Task WriteFileIn(string documentPath, string content) {
            await FileWriter.WriteFileWithContent(documentPath, content + NewLine);
        }

        private string GetFileContentFrom(string documentPath) {
            return File.ReadAllText(documentPath);
        }
    }
}