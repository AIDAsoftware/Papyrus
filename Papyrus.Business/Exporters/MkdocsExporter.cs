using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class MkdocsExporter {
        public async Task Export(string path, WebSite webSite, DirectoryInfo directory) {
            var mkdocsDirectory = directory.CreateSubdirectory(path);
            var docsDirectory = mkdocsDirectory.CreateSubdirectory("docs");
            foreach (var document in webSite.documents) {
                var documentDirectory = docsDirectory.CreateSubdirectory(document.Route);
                var documentPath = Path.Combine(documentDirectory.FullName, document.Title + ".md");
                await FileWriter.WriteFileWithContent(documentPath, document.Content);
            }
            var ymlPath = Path.Combine(mkdocsDirectory.FullName, "mkdocs.yml");
            await FileWriter.WriteFileWithContent(ymlPath, "");
        }
    }
}