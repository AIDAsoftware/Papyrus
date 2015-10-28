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
        public async Task create_two_folders_for_spanish_and_english_documents_into_the_given_directory()
        {
            topicRepository.GetEditableTopicsForProduct(PapyrusId).Returns(Task.FromResult(new List<EditableTopic>()));
            
            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, testDirectory);

            var languagesFolders = testDirectory.GetDirectories().Select(d => d.Name);
            languagesFolders.Should().Contain(SpanishLanguage);
            languagesFolders.Should().Contain(EnglishLanguage);
        }

        [Test]
        public async Task insert_in_each_language_folder_its_respective_topics()
        {
            var topic = TopicForPapyrusWithOneVersionRangeWithAnyDocumentForSpanishAndEnglishLanguage();
            topicRepository.GetEditableTopicsForProduct(PapyrusId).Returns(Task.FromResult(new List<EditableTopic>{topic}));

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, testDirectory);

            var spanishDocument = testDirectory.GetDirectories().First(d => d.Name == SpanishLanguage).GetFiles()[0];
            var englishDocument = testDirectory.GetDirectories().First(d => d.Name == EnglishLanguage).GetFiles()[0];
            spanishDocument.Name.Should().EndWith("Título.md");
            GetContentOf(spanishDocument).Should().Be("Contenido");
            englishDocument.Name.Should().Be("Title.md");
            GetContentOf(englishDocument).Should().Be("Content");
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
            var productVersion = new ProductVersion("version1", "1.0", DateTime.Today);
            var versionRange = new EditableVersionRange
            {
                FromVersion = productVersion,
                ToVersion = productVersion
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