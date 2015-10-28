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
        private TopicQueryRepository topicRepository;
        private readonly DisplayableProduct papyrus = new DisplayableProduct{ ProductId = PapyrusId, ProductName = "Papyrus"};
        private ProductVersion version1 = new ProductVersion("version1", "1.0", DateTime.Today);
        private ProductVersion version2 = new ProductVersion("version2", "2.0", DateTime.Today.AddDays(3));
        private const string SpanishLanguage = "es-ES";
        private const string EnglishLanguage = "en-GB";

        [SetUp]
        public void CreateTestDirectory()
        {
            testDirectory = Directory.CreateDirectory(@"test");
            topicRepository = Substitute.For<TopicQueryRepository>();
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
            var topic = new TopicBuilder()
                .ATopicForProduct(papyrus)
                .WithVersionRange(
                    new VersionRangeBuilder()
                        .VersionRangeFrom(version1)
                        .To(version2)
                        .WithDocument("Título", "Contenido", "es-ES")
                        .WithDocument("Title", "Content", "en-GB")
                        .Build())
                .BuildTopic();
            topicRepository.GetEditableTopicsForProduct(PapyrusId).Returns(Task.FromResult(new List<EditableTopic> { topic }));
            var versions = new List<ProductVersion> { version1, version2 };

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, versions, testDirectory);

            var versionDirectories = testDirectory.GetDirectories();
            var directoryVersion1 = versionDirectories.First(d => d.Name == "1.0");
            var directoryVersion2 = versionDirectories.First(d => d.Name == "2.0");
            var spanishDocumentVersion1 = GetSpanishDocumentFrom(directoryVersion1);
            var spanishDocumentVersion2 = GetSpanishDocumentFrom(directoryVersion2);
            spanishDocumentVersion2.Name.Should().NotBeEmpty("Título.md");
            GetContentOf(spanishDocumentVersion2).Should().Be("Contenido");
            spanishDocumentVersion1.Name.Should().Be("Título.md");
            GetContentOf(spanishDocumentVersion1).Should().Be("Contenido");
        }

        private static FileInfo GetSpanishDocumentFrom(DirectoryInfo directoryVersion1)
        {
            return directoryVersion1
                .GetDirectories()
                .First(d => d.Name == SpanishLanguage)
                .GetFiles().First();
        }

        private static string GetContentOf(FileInfo document)
        {
            return File.ReadAllText(document.FullName);
        }
    }

    public class VersionRangeBuilder
    {
        private EditableVersionRange versionRange;

        public VersionRangeBuilder()
        {
            versionRange = new EditableVersionRange();
        }

        public VersionRangeBuilder VersionRangeFrom(ProductVersion version)
        {
            versionRange.FromVersion = version;
            return this;
        }

        public VersionRangeBuilder To(ProductVersion version)
        {
            versionRange.ToVersion = version;
            return this;
        }

        public VersionRangeBuilder WithDocument(string title, string content, string language)
        {
            versionRange.Documents.Add(new EditableDocument
            {
                Title = title,
                Content = content,
                Language = language
            });
            return this;
        }

        public EditableVersionRange Build()
        {
            return versionRange;
        }
    }

    public class TopicBuilder
    {
        private EditableTopic editableTopic;

        public TopicBuilder()
        {
            editableTopic = new EditableTopic();
        }

        public TopicBuilder ATopicForProduct(DisplayableProduct product)
        {
            editableTopic = new EditableTopic
            {
                Product = product
            };
            return this;
        }

        public TopicBuilder WithVersionRange(EditableVersionRange versionRange)
        {
            editableTopic.VersionRanges.Add(versionRange);
            return this;
        }

        public EditableTopic BuildTopic()
        {
            return editableTopic;
        }
    }
}