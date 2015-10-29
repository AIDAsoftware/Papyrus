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

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class MkdocsExporterShould {
        private const string PapyrusId = "PapyrusId";
        private DirectoryInfo testDirectory;
        private MkDocsExporter mkdocsExporter;
        private TopicQueryRepository topicRepository;
        private readonly DisplayableProduct papyrus = new DisplayableProduct { ProductId = PapyrusId, ProductName = "Papyrus" };
        private readonly ProductVersion version1 = new ProductVersion("version1", "1.0", DateTime.Today);
        private readonly ProductVersion version2 = new ProductVersion("version2", "2.0", DateTime.Today.AddDays(3));
        private readonly ProductVersion version3 = new ProductVersion("version3", "3.0", DateTime.Today.AddDays(4));
        private const string SpanishLanguage = "es-ES";
        private const string EnglishLanguage = "en-GB";

        [SetUp]
        public void CreateTestDirectory() {
            testDirectory = Directory.CreateDirectory(@"test");
            topicRepository = Substitute.For<TopicQueryRepository>();
            mkdocsExporter = new MkDocsExporter(topicRepository);
        }

        [TearDown]
        public void DeleteFolder() {
            testDirectory.Delete(true);
        }

        [Test]
        public async Task insert_in_each_language_folder_its_respective_topics() {
            var topic = new TopicBuilder()
                .ATopicForProduct(papyrus)
                .WithVersionRange(
                    new VersionRangeBuilder()
                        .AddVersion(version1)
                        .AddVersion(version2)
                        .WithDocument("Título", "Contenido", SpanishLanguage)
                        .WithDocument("Title", "Content", EnglishLanguage)
                        .Build())
                .BuildTopic();
            topicRepository.GetEditableTopicsForProduct(PapyrusId).Returns(Task.FromResult(new List<ExportableTopic> { topic }));

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, testDirectory);

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

        [Test]
        public async Task export_documentation_only_for_a_given_version_of_a_product() {
            var firstTopic = new TopicBuilder()
                .ATopicForProduct(papyrus)
                .WithVersionRange(
                    new VersionRangeBuilder()
                        .AddVersion(version3)
                        .WithDocument("Un Título", "Un Contenido", SpanishLanguage)
                        .WithDocument("A Title", "A Content", EnglishLanguage)
                        .Build())
                .BuildTopic();
            topicRepository.GetEditableTopicsForProductVersion(PapyrusId, version3).Returns(Task
                                        .FromResult(new List<ExportableTopic> { firstTopic }));

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, version3, testDirectory);

            testDirectory.GetDirectories().Should().HaveCount(2);
            var spanishDirectory = testDirectory.GetDirectories().First(d => d.Name == SpanishLanguage);
            spanishDirectory.GetFiles().Should().HaveCount(1);
            spanishDirectory.GetDirectories().Should().HaveCount(0);
            var spanishDocument = spanishDirectory.GetFiles().First();
            spanishDocument.Name.Should().Be("Un Título.md");
            GetContentOf(spanishDocument).Should().Be("Un Contenido");
            var englishDirectory = testDirectory.GetDirectories().First(d => d.Name == EnglishLanguage);
            englishDirectory.GetFiles().Should().HaveCount(1);
            englishDirectory.GetDirectories().Should().HaveCount(0);
            var englishDocument = englishDirectory.GetFiles().First();
            englishDocument.Name.Should().Be("A Title.md");
            GetContentOf(englishDocument).Should().Be("A Content");
        }

        private static FileInfo GetSpanishDocumentFrom(DirectoryInfo directoryVersion1) {
            return directoryVersion1
                .GetDirectories()
                .First(d => d.Name == SpanishLanguage)
                .GetFiles().First();
        }

        private static string GetContentOf(FileInfo document) {
            return File.ReadAllText(document.FullName);
        }
    }

    public class VersionRangeBuilder {
        private ExportableVersionRange versionRange;

        public VersionRangeBuilder() {
            versionRange = new ExportableVersionRange();
        }

        public VersionRangeBuilder AddVersion(ProductVersion version) {
            versionRange.AddVersion(version);
            return this;
        }

        public VersionRangeBuilder WithDocument(string title, string content, string language) {
            versionRange.Documents.Add(new Document(title, "", content, language));
            return this;
        }

        public ExportableVersionRange Build() {
            return versionRange;
        }
    }

    public class TopicBuilder {
        private ExportableTopic editableTopic;

        public TopicBuilder() {
            editableTopic = new ExportableTopic();
        }

        public TopicBuilder ATopicForProduct(DisplayableProduct product) {
            editableTopic = new ExportableTopic();
            return this;
        }

        public TopicBuilder WithVersionRange(ExportableVersionRange versionRange) {
            editableTopic.VersionRanges.Add(versionRange);
            return this;
        }

        public ExportableTopic BuildTopic() {
            return editableTopic;
        }
    }
}