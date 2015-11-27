using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class MkdocsExporterShould {
        private const string AnyMkdocsPath = "AnyLanguage/AnyVersion";
        private DirectoryInfo testDirectory;
        // TODO
        //  - should generate exportable document in the docs folder
        //  - should generate proper content for mkdocs.yml

        [SetUp]
        public void SetUp() {
            var path = Path.GetTempPath();
            var testDirectoryPath = Path.Combine(path, Guid.NewGuid().ToString());
            testDirectory = Directory.CreateDirectory(testDirectoryPath);
        }

        [TearDown]
        public void TearDown() {
            testDirectory.Delete(true);
        }

        [Test]
        public async Task generate_mkdocs_yml_file_in_the_given_path() {
            var webSite = WebSiteWithDocument(AnyDocument());

            await new MkdocsExporter().Export(webSite, GetAnyExportationPath());

            GetFilesFrom(AnyMkdocsPath).Should().Contain(x => x.Name == "mkdocs.yml");
        }

        [Test]
        public async Task generate_docs_folder_in_the_given_folder() {
            var webSite = WebSiteWithDocument(AnyDocument());

            await new MkdocsExporter().Export(webSite, GetAnyExportationPath());

            GetFoldersFrom(AnyMkdocsPath).Should().Contain(x => x.Name == "docs");
        }

        [Test]
        public async Task generate_exportable_document_in_the_docs_folder() {
            var documentContent = "Content";
            var webSite = WebSiteWithDocument(new ExportableDocument("Title", documentContent, "Product"));

            await new MkdocsExporter().Export(webSite, GetAnyExportationPath());

            var documentPath = Path.Combine(GetAnyExportationPath(), "docs/Product/Title.md");
            GetFileContentFrom(documentPath).Should().Be(documentContent);
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
            return new ExportableDocument("AnyTitle", "AnyContent", "AnyWebsitePath");
        }

        private static WebSite WebSiteWithDocument(ExportableDocument exportableDocument) {
            var webSite = new WebSite();
            webSite.AddDocument(exportableDocument);
            return webSite;
        }
    }
}