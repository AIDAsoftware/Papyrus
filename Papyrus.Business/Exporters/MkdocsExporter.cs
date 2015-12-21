using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class MkdocsExporter {
        private const string YmlFileName = "mkdocs.yml";
        private const string MarkDownExtension = ".md";
        private readonly string mkdocsTheme = "theme: readthedocs" + System.Environment.NewLine;
        private readonly string siteName = "site_name: SIMA Documentation" + System.Environment.NewLine;
        public const string IndexContent = "SIMA Documentation";

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
            await WriteFileIn(ymlPath, mkdocsTheme + siteName);
            await WriteFileIn(ymlPath, "pages:" + System.Environment.NewLine);
            await WriteFileIn(ymlPath, document.ExportableTitle + ": " + document.Title);
        }

        private static async Task ExportDocumentIn(ExportableDocument document, DirectoryInfo directory) {
            var documentDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, document.Route));
            var documentPath = Path.Combine(documentDirectory.FullName, document.ExportableTitle + MarkDownExtension);
            await WriteFileIn(documentPath, document.Content);
        }

        private static async Task WriteFileIn(string documentPath, string content) {
            await FileWriter.WriteFileWithContent(documentPath, content);
        }
    }
}