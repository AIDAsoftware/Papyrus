using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class MkdocsExporterShould {
        private const string AnyMkdocsPath = "AnyLanguage/AnyVersion";
        private DirectoryInfo testDirectory;
        private readonly string newLine = Environment.NewLine;
        private const string AnyImagesPath = "AnyImagesPath";
        private FileSystemImagesCopier imagesCopier;


        [SetUp]
        public void SetUp() {
            var path = Path.GetTempPath();
            var testDirectoryPath = Path.Combine(path, Guid.NewGuid().ToString());
            testDirectory = Directory.CreateDirectory(testDirectoryPath);
            imagesCopier = Substitute.For<FileSystemImagesCopier>();
        }

        [TearDown]
        public void TearDown() {
            testDirectory.Delete(true);
        }

        [Test]
        public async Task generate_mkdocs_yml_file_in_the_given_path() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(path, AnyImagesPath));

            GetFilesFrom(AnyMkdocsPath).Should().Contain(x => x.Name == "mkdocs.yml");
        }

        [Test]
        public async Task generate_documents_in_docs_file_naming_as_index_the_first_one() {
            var webSite = WebSiteWithDocuments(new ExportableDocument("Title", "Content"));

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(path, AnyImagesPath));

            var content = GetFileContentFrom(Path.Combine(GetAnyExportationPath(), "docs/index.md"));
            content.Should().Be("Content" + Environment.NewLine);
        }
        
        [Test]
        public async Task generate_docs_folder_in_the_given_folder() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(path, AnyImagesPath));

            GetFoldersFrom(AnyMkdocsPath).Should().Contain(x => x.Name == "docs");
        }

        [Test]
        public async Task write_the_theme_in_the_yml_file() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(path, AnyImagesPath));

            var documentPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(documentPath).Should().Contain("theme: readthedocs");
        }
        
        [Test]
        public async Task write_the_site_name_in_the_yml_file() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(path, AnyImagesPath));

            var documentPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(documentPath).Should().Contain("site_name: SIMA Documentation");
        }
        
        [Test]
        public async Task write_the_index_file_into_docs_directory() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(path, AnyImagesPath));

            GetDocsFrom(GetAnyExportationPath()).Should().Contain(d => d.Name == "index.md");
        }

        [Test]
        public async Task replace_unavailabe_characters_for_a_file_for_available_ones() {
            var website = WebSiteWithDocuments(
                new ExportableDocument("this/is|the*Title", "AnyContent"),
                new ExportableDocument("otro>título?ñ", "AnotherContent"));

            string path = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(website, new ConfigurationSettings(path, AnyImagesPath));

            var ymlPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(ymlPath).Should().Contain(
                "pages: " + newLine +
                "- 'this/is|the*Title': 'index.md'" + newLine +
                "- 'otro>título?ñ': 'otro-titulo-n.md'");
        }
        
        [Test]
        public async Task copy_images_to_website_folder() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            var anyExportationPath = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(anyExportationPath, AnyImagesPath));

            imagesCopier.Received(1).CopyFolder(AnyImagesPath, Path.Combine(anyExportationPath, "docs", AnyImagesPath));
        }

        [Test]
        public async Task dont_override_the_index_when_exist_a_collision_between_two_documents_to_be_the_first()
        {
            var aDocument = AnyDocument();
            var anotherDocument = AnyDocument();
            var webSite = WebSiteWithDocuments(aDocument, anotherDocument);

            var anyExportationPath = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, new ConfigurationSettings(anyExportationPath, AnyImagesPath));

            GetDocsFrom(GetAnyExportationPath()).Should().HaveCount(2)
                .And.Contain(d => d.Name == "index.md")
                .And.Contain(d => d.Name.Contains(aDocument.Title) || d.Name.Contains(anotherDocument.Title));
        }

        [Test]
        public async Task configure_google_analytics_in_the_yml_file()
        {
            var webSite = WebSiteWithDocuments(AnyDocument());
            string path = GetAnyExportationPath();
            var googleAnalyticsId = "GoogleAnalyticsId";
            var settings = new ConfigurationSettings(path, AnyImagesPath, googleAnalyticsId: googleAnalyticsId);

            await new MkDocsExporter(imagesCopier).Export(webSite, settings);

            var documentPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(documentPath).Should().Contain("google_analytics: ['" + googleAnalyticsId + "', 'auto']");
        }

        private string GetAnyExportationPath() {
            return Path.Combine(testDirectory.FullName, AnyMkdocsPath);
        }

        private string GetFileContentFrom(string documentPath) {
            return File.ReadAllText(documentPath);
        }

        private FileInfo[] GetDocsFrom(string path) {
            var docsPath = Path.Combine(path, "docs");
            return new DirectoryInfo(Path.Combine(testDirectory.FullName, docsPath)).GetFiles();
        }

        private DirectoryInfo[] GetFoldersFrom(string path) {
            return new DirectoryInfo(Path.Combine(testDirectory.FullName, path)).GetDirectories();
        }

        private FileInfo[] GetFilesFrom(string path) {
            return new DirectoryInfo(Path.Combine(testDirectory.FullName, path)).GetFiles();
        }

        private static ExportableDocument AnyDocument() {
            return new ExportableDocument(AnyUniqueString(), "AnyContent");
        }

        private static string AnyUniqueString() {
            return Guid.NewGuid().ToString();
        }

        private static WebSite WebSiteWithDocuments(params ExportableDocument[] exportableDocuments) {
            return new WebSite(exportableDocuments, "AnyProductName");
        }
    }
}