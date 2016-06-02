using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private const string YmlFileName = "mkdocs.yml";
        private const string MarkDownExtension = ".md";
        private readonly string mkdocsTheme = "theme: readthedocs";
        private readonly string siteName = "site_name: SIMA Documentation";
        private static readonly string NewLine = System.Environment.NewLine;
        private readonly FileSystemImagesCopier imagesCopier;
        public const string IndexContent = "SIMA Documentation";
        private const string NewListItem = "- ";

        public MkDocsExporter(FileSystemImagesCopier imagesCopier) {
            this.imagesCopier = imagesCopier;
        }

        public virtual async Task Export(WebSite webSite, string path, string imagesFolder) {
            var configuration = new MkdocsConfiguration();
            await InitializeMkdocsStructure(path, configuration);
            var docsPath = Path.Combine(path, "docs");
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, docsPath);
                await AddDocumentToTheConfiguration(document, configuration);
            }
            var ymlPath = Path.Combine(path, YmlFileName);
            await WriteInFile(ymlPath, configuration.ToString());
            var imagesFolderDirectoryName = (new DirectoryInfo(imagesFolder)).Name;
            var newImagesDestination = Path.Combine(docsPath, imagesFolderDirectoryName);
            if (!Directory.Exists(newImagesDestination)) {
                imagesCopier.CopyFolder(imagesFolder, newImagesDestination);
            }
        }

        private async Task InitializeMkdocsStructure(string path, MkdocsConfiguration configuration) {
            var docsPath = Path.Combine(path, "docs");
            var docsDirectory = Directory.CreateDirectory(docsPath);
            await WriteInFile(Path.Combine(docsDirectory.FullName, "index.md"), IndexContent);
            AddIndexPageTo(configuration);
        }

        private static void AddIndexPageTo(MkdocsConfiguration configuration) {
            configuration.AddPage("Home", "index.md");
        }

        private static async Task AddDocumentToTheConfiguration(ExportableDocument document, MkdocsConfiguration configuration) {
            configuration.AddPage(document.Title, Path.Combine(document.Route, ConvertToValidFileName(document.Title)) + MarkDownExtension);
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