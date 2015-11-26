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
        private DirectoryInfo testDirectory;
        // TODO
        //  - should generate docs folder in the given folder
        //  - should generate exportable document in the docs folder

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
            var mkdocsPath = "anyLanguage/AnyVersion";
            var webSite = WebSiteWithDocument(AnyDocument());

            await new MkdocsExporter().Export(mkdocsPath, webSite, testDirectory);

            GetFilesFrom(mkdocsPath).Should().Contain(x => x.Name == "mkdocs.yml");
        }

        [Test]
        public async Task generate_docs_folder_in_the_given_folder() {
            var mkdocsPath = "anyLanguage/AnyVersion";
            var webSite = WebSiteWithDocument(AnyDocument());

            await new MkdocsExporter().Export(mkdocsPath, webSite, testDirectory);

            GetFoldersFrom(mkdocsPath).Should().Contain(x => x.Name == "docs");
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