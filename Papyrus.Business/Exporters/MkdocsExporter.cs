using System.IO;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private const string YmlFileName = "mkdocs.yml";
        private const string MarkDownExtension = ".md";
        private static readonly string NewLine = System.Environment.NewLine;
        private readonly FileSystemImagesCopier imagesCopier;
        public const string IndexContent = "SIMA Documentation";

        public MkDocsExporter(FileSystemImagesCopier imagesCopier) {
            this.imagesCopier = imagesCopier;
        }

        public virtual async Task Export(WebSite webSite, ConfigurationPaths configurationPaths) {
            var configuration = CreateConfiguration(configurationPaths.SiteDir);
            var exportationPath = configurationPaths.ExportationPath;
            await InitializeMkdocsStructure(
                exportationPath, configuration);
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, 
                    DocsPathIn(exportationPath));
                await AddDocumentToTheConfiguration(document, configuration);
            }
            await WriteConfigurationYmlInPath(configuration, exportationPath);
            CopyImagesInTheSite(
                exportationPath, 
                configurationPaths.ImagesFolder);
        }

        private void CopyImagesInTheSite(string path, string imagesFolder) {
            var newImagesDestination = ConstructNewImagesDestination(path, imagesFolder);
            if (!Directory.Exists(newImagesDestination)) {
                imagesCopier.CopyFolder(imagesFolder, newImagesDestination);
            }
        }

        private static string ConstructNewImagesDestination(string path, string imagesFolder) {
            var imagesFolderDirectoryName = (new DirectoryInfo(imagesFolder)).Name;
            var newImagesDestination = Path.Combine(DocsPathIn(path), imagesFolderDirectoryName);
            return newImagesDestination;
        }

        private static string DocsPathIn(string path) {
            return Path.Combine(path, "docs");
        }

        private static MkdocsConfiguration CreateConfiguration(string siteDir) {
            return new MkdocsConfiguration(siteDir);
        }

        private static async Task WriteConfigurationYmlInPath(MkdocsConfiguration configuration, string path) {
            var ymlPath = Path.Combine(path, YmlFileName);
            File.Delete(ymlPath);
            await WriteInFile(ymlPath, configuration.ToString());
        }

        private async Task InitializeMkdocsStructure(string path, MkdocsConfiguration configuration) {
            var docsPath = Path.Combine(path, "docs");
            var docsDirectory = Directory.CreateDirectory(docsPath);
            await WriteInFile(Path.Combine(docsDirectory.FullName, "index.md"), IndexContent);
            AddIndexPageTo(configuration);
        }

        private static void AddIndexPageTo(MkdocsConfiguration configuration) {
            configuration.AddPage("Home", new ExportableDocument("index", string.Empty, 0));
        }

        private static async Task AddDocumentToTheConfiguration(ExportableDocument document, MkdocsConfiguration configuration) {
            configuration.AddPage(document.Title, document);
        }

        private static async Task ExportDocumentIn(ExportableDocument document, string directoryPath) {
            var documentDirectory = Directory.CreateDirectory(directoryPath);
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