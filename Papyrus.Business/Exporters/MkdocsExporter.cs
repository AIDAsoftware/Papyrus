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
            await InitializeMkdocsStructure(path);
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, Path.Combine(path, "docs"));
                await GenerateDocInYml(document, Path.Combine(path, YmlFileName));
            }
        }

        private async Task InitializeMkdocsStructure(string path) {
            var docsPath = Path.Combine(path, "docs");
            var docsDirectory = Directory.CreateDirectory(docsPath);
            await WriteInFile(Path.Combine(docsDirectory.FullName, "index.md"), IndexContent);
            await InitializeYmlFileIn(path);
        }

        private async Task InitializeYmlFileIn(string path) {
            if (File.Exists(Path.Combine(path, YmlFileName))) return;
            var ymlPath = Path.Combine(path, YmlFileName);
            await WriteInFile(ymlPath, mkdocsTheme);
            await WriteInFile(ymlPath, siteName);
            await WriteInFile(ymlPath, "pages:");
            await WriteInFile(ymlPath, "- 'Home': 'index.md'");
        }

        private static async Task GenerateDocInYml(ExportableDocument document, string ymlPath) {
            var docReference = MkdocsPagePresentationFor(document);
            if (string.IsNullOrEmpty(document.Route)) {
                await WriteInFile(ymlPath, docReference);
                return;
            }
            if (!ReadContentOf(ymlPath).Contains(document.Route)) {
                await WriteInFile(ymlPath, NewListItemWith(document.Route));
            }
            await WriteInFile(ymlPath, "    " + docReference);
        }

        private static string ReadContentOf(string ymlPath) {
            return File.ReadAllText(ymlPath);
        }

        private static string NewListItemWith(string itemContent) {
            return NewListItem + "'" + itemContent + "':";
        }

        private static string MkdocsPagePresentationFor(ExportableDocument document) {
            return NewListItem + "'" + document.Title + "': '" + 
                Path.Combine(document.Route, ConvertToValidFileName(document.Title)) + MarkDownExtension + "'";
        }

        private static async Task ExportDocumentIn(ExportableDocument document, string directoryPath) {
            var documentDirectory = Directory.CreateDirectory(Path.Combine(directoryPath, document.Route));
            var documentPath = Path.Combine(documentDirectory.FullName, ConvertToValidFileName(document.Title) + MarkDownExtension);
            await WriteInFile(documentPath, document.Content);
        }

        private static async Task WriteInFile(string documentPath, string content) {
            await FileWriter.WriteFileWithContent(documentPath, content + NewLine);
        }

        private static string ConvertToValidFileName(string title) {
            return MkdocsFileNameConverter.ConvertToValidFileName(title);
        }
    }
}