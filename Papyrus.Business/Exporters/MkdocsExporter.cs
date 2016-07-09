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
            var exportationPath = Path.Combine(configurationPaths.ExportationPath, GenerateMkdocsPath(webSite));
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

        public virtual string GenerateMkdocsPath(WebSite webSite) {
            const string separator = "/";
            return webSite.Version + separator + webSite.ProductName + separator + webSite.Language;
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
            await WriteInFile(ymlPath, configuration.ToString());
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
            configuration.AddPage(document.Title, ConvertToValidFileName(document.Title) + MarkDownExtension);
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

    public class ConfigurationPaths {
        public string ExportationPath { get; }
        public string ImagesFolder { get; }
        public string SiteDir { get; }

        public ConfigurationPaths(string exportationPath, string imagesFolder, string siteDir = "") {
            this.ExportationPath = exportationPath;
            this.ImagesFolder = imagesFolder;
            this.SiteDir = siteDir;
        }

        protected bool Equals(ConfigurationPaths other)
        {
            return string.Equals(ExportationPath, other.ExportationPath) && 
                string.Equals(ImagesFolder, other.ImagesFolder) && 
                string.Equals(SiteDir, other.SiteDir);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConfigurationPaths) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ExportationPath != null ? ExportationPath.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ImagesFolder != null ? ImagesFolder.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SiteDir != null ? SiteDir.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}