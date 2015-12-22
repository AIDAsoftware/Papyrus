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
        private const string NewListItem = "- ";

        public virtual async Task Export(WebSite webSite, string path) {
            var docsPath = Path.Combine(path, "docs");
            var docsDirectory = Directory.CreateDirectory(docsPath);
            await WriteInFile(Path.Combine(docsDirectory.FullName, "index.md"), IndexContent);
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, docsDirectory);
                await WriteYmlFileIn(path, document);
            }
        }

        private async Task WriteYmlFileIn(string path, ExportableDocument document) {
            var ymlPath = Path.Combine(path, YmlFileName);
            if (!File.Exists(ymlPath)) {
                await InitializeMkdocsYmlFile(ymlPath);
            }
            if (string.IsNullOrEmpty(document.Route)) {
                await WriteInFile(ymlPath, MkdocsPagePresentationFor(document));
                return;
            }
            if (File.ReadAllText(ymlPath).Contains("- " + document.Route)) {
                await WriteInFile(ymlPath, "\t" + MkdocsPagePresentationFor(document));
                return;
            }
            await WriteInFile(ymlPath, NewListItem + document.Route + ":");
            await WriteInFile(ymlPath, "\t" + MkdocsPagePresentationFor(document));
        }

        private async Task InitializeMkdocsYmlFile(string ymlPath) {
            await WriteInFile(ymlPath, mkdocsTheme);
            await WriteInFile(ymlPath, siteName);
            await WriteInFile(ymlPath, "pages:");
        }

        private static string MkdocsPagePresentationFor(ExportableDocument document) {
            return NewListItem + document.Title + ": " + Path.Combine(document.Route, document.ExportableTitle) + MarkDownExtension;
        }

        private static async Task ExportDocumentIn(ExportableDocument document, DirectoryInfo directory) {
            var documentDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, document.Route));
            var documentPath = Path.Combine(documentDirectory.FullName, document.ExportableTitle + MarkDownExtension);
            await WriteInFile(documentPath, document.Content);
        }

        private static async Task WriteInFile(string documentPath, string content) {
            await FileWriter.WriteFileWithContent(documentPath, content + NewLine);
        }
    }
}