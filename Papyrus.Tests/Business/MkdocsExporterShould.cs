using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Exporters;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class MkdocsExporterShould
    {
        // TODO:
        //   - create a folder for spanish and english documents
        //   - insert in each of these folders topics its respective topics

        private const string PapyrusId = "PapyrusId";
        private DirectoryInfo testDirectory;
        private MkDocsExporter mkdocsExporter;
        private TopicRepository topicRepository;
        private const string SpanishLanguage = "es-ES";
        private const string EnglishLanguage = "en-GB";

        [SetUp]
        public void CreateTestDirectory()
        {
            testDirectory = Directory.CreateDirectory(@"test");
            topicRepository = Substitute.For<TopicRepository>();
            mkdocsExporter = new MkDocsExporter(topicRepository);
        }

        [TearDown]
        public void DeleteFolder()
        {
            testDirectory.Delete(true);
        }

        [Test]
        public async Task insert_in_each_language_folder_its_respective_topics()
        {
            var topic = TopicForPapyrusWithOneVersionRangeWithAnyDocumentForSpanishAndEnglishLanguage();
            topicRepository.GetEditableTopicsForProduct(PapyrusId).Returns(Task.FromResult(new List<EditableTopic> { topic }));
            var versions = new List<ProductVersion>
            {
                new ProductVersion("version1", "1.0", DateTime.Today),
                new ProductVersion("version2", "2.0", DateTime.Today.AddDays(3))
            };

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, versions, testDirectory);

            var directoryVersion1 = testDirectory.GetDirectories().First(d => d.Name == "1.0");
            var directoryVersion2 = testDirectory.GetDirectories().First(d => d.Name == "2.0");
            var spanishDocumentVersion1 = directoryVersion1.GetDirectories().First(d => d.Name == SpanishLanguage)
                                                            .GetFiles().First();
            var spanishDocumentVersion2 = directoryVersion2.GetDirectories().First(d => d.Name == SpanishLanguage)
                                                            .GetFiles().First();
            spanishDocumentVersion2.Name.Should().EndWith("Título.md");
            GetContentOf(spanishDocumentVersion2).Should().Be("Contenido");
            spanishDocumentVersion1.Name.Should().Be("Título.md");
            GetContentOf(spanishDocumentVersion1).Should().Be("Contenido");
        }

        private static string GetContentOf(FileInfo document)
        {
            return File.ReadAllText(document.FullName);
        }

        private static EditableTopic TopicForPapyrusWithOneVersionRangeWithAnyDocumentForSpanishAndEnglishLanguage()
        {
            var product = new DisplayableProduct
            {
                ProductId = "ProductId",
                ProductName = "Papyrus",
            };
            var versionRange = new EditableVersionRange
            {
                FromVersion = new ProductVersion("version1", "1.0", DateTime.Today),
                ToVersion = new ProductVersion("version2", "2.0", DateTime.Today.AddDays(3))
            };
            versionRange.Documents.Add(new EditableDocument
            {
                Title = "Título",
                Description = "Descripción",
                Content = "Contenido",
                Language = SpanishLanguage
            });
            versionRange.Documents.Add(new EditableDocument
            {
                Title = "Title",
                Description = "Description",
                Content = "Content",
                Language = EnglishLanguage
            });
            var topic = new EditableTopic
            {
                Product = product,
                TopicId = "TopicId",
                VersionRanges = new ObservableCollection<EditableVersionRange>() {versionRange}
            };
            return topic;
        }
    }
}