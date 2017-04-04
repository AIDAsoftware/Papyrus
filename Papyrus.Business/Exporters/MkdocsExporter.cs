using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private const string YmlFileName = "mkdocs.yml";
        private const string MarkDownExtension = ".md";
        private static readonly string NewLine = System.Environment.NewLine;
        private readonly FileSystemImagesCopier imagesCopier;
        private int firstDocumentOrder;

        public MkDocsExporter(FileSystemImagesCopier imagesCopier) {
            this.imagesCopier = imagesCopier;
        }

        public virtual async Task Export(WebSite webSite, ConfigurationSettings configurationSettings) {
            var configuration = CreateConfiguration(configurationSettings.SiteDir, configurationSettings);
            var exportationPath = configurationSettings.ExportationPath;
            await InitializeMkdocsStructure(
                exportationPath, configuration);
            firstDocumentOrder = webSite.Documents.OrderBy(d => d.Order).First().Order;
            foreach (var document in webSite.Documents) {
                await ExportDocumentIn(document, 
                    DocsPathIn(exportationPath));
                await AddDocumentToTheConfiguration(document, configuration);
            }
            await WriteConfigurationYmlInPath(configuration, exportationPath);
            CopyImagesInTheSite(
                exportationPath, 
                configurationSettings.ImagesFolder);
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

        private static MkdocsConfiguration CreateConfiguration(string siteDir, ConfigurationSettings configurationSettings) {
            return new MkdocsConfiguration(configurationSettings.SiteDir, configurationSettings.GoogleAnalyticsId);
        }

        private static async Task WriteConfigurationYmlInPath(MkdocsConfiguration configuration, string path) {
            var ymlPath = Path.Combine(path, YmlFileName);
            File.Delete(ymlPath);
            await WriteInFile(ymlPath, configuration.ToString());
        }

        private async Task InitializeMkdocsStructure(string path, MkdocsConfiguration configuration) {
            var docsPath = Path.Combine(path, "docs");
            Directory.CreateDirectory(docsPath);
        }

        private static async Task AddDocumentToTheConfiguration(ExportableDocument document, MkdocsConfiguration configuration) {
            configuration.AddPage(document.Title, document);
        }

        private async Task ExportDocumentIn(ExportableDocument document, string directoryPath) {
            var documentDirectory = Directory.CreateDirectory(directoryPath);
            var fileName = (IsFirstDocument(document, directoryPath)) ? 
                            "index.md" : ConvertToValidFileName(document.Title) + MarkDownExtension;
            var documentPath = Path.Combine(documentDirectory.FullName, fileName);
            await WriteInFile(documentPath, document.Content);
        }

        private bool IsFirstDocument(ExportableDocument document, string directoryPath) {
            return document.Order == firstDocumentOrder && !File.Exists(Path.Combine(directoryPath, "index.md"));
        }

        private static async Task WriteInFile(string documentPath, string content) {
            await FileWriter.WriteFileWithContent(documentPath, content + NewLine);
        }

        private static string ConvertToValidFileName(string title) {
            return MkdocsFileNameConverter.ConvertToValidFileName(title);
        }
    }
}