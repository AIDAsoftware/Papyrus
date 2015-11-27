using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class MkdocsExporter {
        public async Task Export(WebSite webSite, string path) {
            var directory = Directory.CreateDirectory(path);
            foreach (var document in webSite.documents) {
                await ExportDocumentIn(document, directory);
            }
            var ymlPath = Path.Combine(path, "mkdocs.yml");
            await FileWriter.WriteFileWithContent(ymlPath, "");
        }

        private static async Task ExportDocumentIn(ExportableDocument document, DirectoryInfo directory) {
            var docsDirectory = directory.CreateSubdirectory("docs");
            var documentDirectory = docsDirectory.CreateSubdirectory(document.Route);
            var documentPath = Path.Combine(documentDirectory.FullName, document.Title + ".md");
            await FileWriter.WriteFileWithContent(documentPath, document.Content);
        }
    }
}