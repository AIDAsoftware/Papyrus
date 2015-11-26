using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Exporters {
    public class MkdocsExporter {
        public async Task Export(string path, WebSite webSite, DirectoryInfo directory) {
            var mkdocsDirectory = directory.CreateSubdirectory(path);
            var ymlPath = Path.Combine(mkdocsDirectory.FullName, "mkdocs.yml");
            await FileWriter.WriteFileWithContent(ymlPath, "");
        }
    }
}