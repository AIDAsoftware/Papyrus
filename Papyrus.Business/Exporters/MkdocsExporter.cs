using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class MkdocsExporter {
        private const string YmlFileName = "mkdocs.yml";
        private const string MarkDownExtension = ".md";
        private static readonly string MkdocsTheme = "theme: readthedocs" + System.Environment.NewLine;
        private static readonly string SiteNAme = "site_name: SIMA Documentation" + System.Environment.NewLine;
        public const string IndexContent = "SIMA Documentation";

        public virtual async Task Export(WebSite webSite, string path) {
            var docsPath = Path.Combine(path, "docs");
            var docsDirectory = Directory.CreateDirectory(docsPath);
            await FileWriter.WriteFileWithContent(Path.Combine(docsDirectory.FullName, "index.md"), IndexContent);
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, docsDirectory);
            }
            await WriteYmlFileIn(path);
        }

        private static async Task WriteYmlFileIn(string path) {
            var ymlPath = Path.Combine(path, YmlFileName);
            await FileWriter.WriteFileWithContent(ymlPath, MkdocsTheme + SiteNAme);
        }

        private static async Task ExportDocumentIn(ExportableDocument document, DirectoryInfo directory) {
            var documentDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, document.Route));
            var documentPath = Path.Combine(documentDirectory.FullName, document.Title + MarkDownExtension);
            await FileWriter.WriteFileWithContent(documentPath, document.Content);
        }
    }
}