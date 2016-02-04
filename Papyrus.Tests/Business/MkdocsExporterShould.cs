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

            await new MkDocsExporter(imagesCopier).Export(webSite, GetAnyExportationPath(), AnyImagesPath);

            GetFilesFrom(AnyMkdocsPath).Should().Contain(x => x.Name == "mkdocs.yml");
        }

        [Test]
        public async Task generate_documents_in_docs_file() {
            var webSite = WebSiteWithDocuments(new ExportableDocument("Title", "Content", ""));

            await new MkDocsExporter(imagesCopier).Export(webSite, GetAnyExportationPath(), AnyImagesPath);

            var content = GetFileContentFrom(Path.Combine(GetAnyExportationPath(), "docs/Title.md"));
            content.Should().Be("Content" + Environment.NewLine);
        }
        
        [Test]
        public async Task generate_docs_folder_in_the_given_folder() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            await new MkDocsExporter(imagesCopier).Export(webSite, GetAnyExportationPath(), AnyImagesPath);

            GetFoldersFrom(AnyMkdocsPath).Should().Contain(x => x.Name == "docs");
        }

        [Test]
        public async Task write_the_theme_in_the_yml_file() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            await new MkDocsExporter(imagesCopier).Export(webSite, GetAnyExportationPath(), AnyImagesPath);

            var documentPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(documentPath).Should().Contain("theme: readthedocs");
        }
        
        [Test]
        public async Task write_the_site_name_in_the_yml_file() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            await new MkDocsExporter(imagesCopier).Export(webSite, GetAnyExportationPath(), AnyImagesPath);

            var documentPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(documentPath).Should().Contain("site_name: SIMA Documentation");
        }
        
        [Test]
        public async Task write_the_index_file_into_docs_directory() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            await new MkDocsExporter(imagesCopier).Export(webSite, GetAnyExportationPath(), AnyImagesPath);

            GetDocsFrom(GetAnyExportationPath()).Should().Contain(d => d.Name == "index.md");
        }

        [Test]
        public async Task replace_unavailabe_characters_for_a_file_for_available_ones() {
            var website = WebSiteWithDocuments(
                new ExportableDocument("this/is|the*Title", "AnyContent", ""),
                new ExportableDocument("otro>título?ñ", "AnotherContent", ""));

            await new MkDocsExporter(imagesCopier).Export(website, GetAnyExportationPath(), AnyImagesPath);

            var ymlPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(ymlPath).Should().Contain(
                "pages:" + newLine +
                "- 'Home': 'index.md'" + newLine +
                "- 'this/is|the*Title': 'this-is-the-Title.md'" + newLine +
                "- 'otro>título?ñ': 'otro-titulo-n.md'");
        }
        
        [Test]
        public async Task replace_unavailabe_characters_for_a_file_for_available_ones_when_have_a_route_in_each_document() {
            var firstWebsite = WebSiteWithDocuments(
                new ExportableDocument("first-file", "AnyContent", "first-route"),
                new ExportableDocument("second>file", "AnotherContent", "first-route"));
            var secondWebsite = WebSiteWithDocuments(
                new ExportableDocument("third>file", "MoreContent", "second-route"));

            await new MkDocsExporter(imagesCopier).Export(firstWebsite, GetAnyExportationPath(), AnyImagesPath);
            await new MkDocsExporter(imagesCopier).Export(secondWebsite, GetAnyExportationPath(), AnyImagesPath);

            var ymlPath = Path.Combine(GetAnyExportationPath(), "mkdocs.yml");
            GetFileContentFrom(ymlPath).Should().Contain(
                "pages:" + newLine +
                "- 'Home': 'index.md'" + newLine +
                "- 'first-route':" + newLine +
                "    - 'first-file': 'first-route\\first-file.md'" + newLine +
                "    - 'second>file': 'first-route\\second-file.md'" + newLine +
                "- 'second-route':" + newLine + 
                "    - 'third>file': 'second-route\\third-file.md'");
        }

        [Test]
        public async Task copy_images_to_website_folder() {
            var webSite = WebSiteWithDocuments(AnyDocument());

            var anyExportationPath = GetAnyExportationPath();
            await new MkDocsExporter(imagesCopier).Export(webSite, anyExportationPath, AnyImagesPath);

            imagesCopier.Received(1).CopyFolder(AnyImagesPath, Path.Combine(anyExportationPath, "docs", AnyImagesPath));
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
            return new ExportableDocument("AnyTitle", "AnyContent", "/AnyWebsitePath");
        }

        private static WebSite WebSiteWithDocuments(params ExportableDocument[] exportableDocuments) {
            return new WebSite(exportableDocuments);
        }
    }
}