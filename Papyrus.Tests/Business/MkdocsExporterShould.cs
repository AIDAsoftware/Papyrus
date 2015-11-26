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
            var testDirectoryPath = Directory.GetCurrentDirectory();
            testDirectory = Directory.CreateDirectory(testDirectoryPath);
        }

        [Test]
        public async Task generate_mkdocs_yml_file_in_the_given_path() {
            var path = "anyLanguage/AnyVersion";
            var webSite = new WebSite();
            webSite.AddDocument(new ExportableDocument("AnyTitle", "AnyContent", "AnyWebsitePath"));

            await new MkdocsExporter().Export(path, webSite, testDirectory);

            var mkdocsDirectoryFiles = new DirectoryInfo(Path.Combine(testDirectory.FullName, path)).GetFiles(); 
            mkdocsDirectoryFiles.Should().Contain(x => x.Name == "mkdocs.yml");
        }
    }
}