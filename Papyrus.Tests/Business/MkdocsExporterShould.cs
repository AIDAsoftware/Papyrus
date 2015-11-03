﻿using System;
using System.Collections.Generic;
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
        private readonly ExportableProduct papyrus = new ExportableProduct(PapyrusId, "Papyrus");
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
            topicRepository.GetExportableTopicsForProduct(PapyrusId).Returns(Task.FromResult(new List<ExportableTopic> { topic }));

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, testDirectory);

            var versionDirectories = testDirectory.GetDirectories();
            var versionDirectory = versionDirectories.First(d => d.Name == "1.0");
            var productDirectory = versionDirectory.GetDirectories().First(d => d.Name == "Papyrus");
            var spanishDirectory = productDirectory.GetDirectories().First(d => d.Name == "es-ES");
            spanishDirectory.GetFiles().Should().Contain(d => d.Name == "mkdocs.yml");
            var docsDirectory = spanishDirectory.GetDirectories().First(d => d.Name == "docs");
            docsDirectory.GetFiles().Should().Contain(f => f.Name == "index.md");
            var documentDirectory = docsDirectory.GetFiles().First(f => f.Name == "Título.md");
            GetContentOf(documentDirectory).Should().Be("Contenido");
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
            topicRepository.GetExportableTopicsForProductVersion(PapyrusId, version3).Returns(Task
                                        .FromResult(new List<ExportableTopic> { firstTopic }));

            await mkdocsExporter.ExportDocumentsForProductToFolder(PapyrusId, version3, testDirectory);

            var versionDirectory = testDirectory.GetDirectories().First(d => d.Name == "3.0");
            versionDirectory.GetDirectories().Should().HaveCount(1);
            var productDirectory = versionDirectory.GetDirectories().First(d => d.Name == "Papyrus");
            var spanishDirectory = productDirectory.GetDirectories().First(d => d.Name == SpanishLanguage);
            spanishDirectory.GetFiles().Should().Contain(f => f.Name == "mkdocs.yml");
            spanishDirectory.GetDirectories().Should().HaveCount(1);
            var spanishDocsDirectory = spanishDirectory.GetDirectories().First(d => d.Name == "docs");
            var spanishDocument = spanishDocsDirectory.GetFiles().First(f => f.Name == "Un Título.md");
            GetContentOf(spanishDocument).Should().Be("Un Contenido");
            var englishDirectory = productDirectory.GetDirectories().First(d => d.Name == EnglishLanguage);
            englishDirectory.GetFiles().Should().Contain(f => f.Name == "mkdocs.yml");
            englishDirectory.GetDirectories().Should().HaveCount(1);
            var englishDocsDirectory = englishDirectory.GetDirectories().First(d => d.Name == "docs");
            var englishDocument = englishDocsDirectory.GetFiles().First(f => f.Name == "A Title.md");
            GetContentOf(englishDocument).Should().Be("A Content");
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
            versionRange.Documents.Add(new ExportableDocument(title, content, language));
            return this;
        }

        public ExportableVersionRange Build() {
            return versionRange;
        }
    }

    public class TopicBuilder {
        private ExportableTopic exportableTopic;

        public TopicBuilder ATopicForProduct(ExportableProduct product) {
            exportableTopic = new ExportableTopic(product);
            return this;
        }

        public TopicBuilder WithVersionRange(ExportableVersionRange versionRange) {
            exportableTopic.VersionRanges.Add(versionRange);
            return this;
        }

        public ExportableTopic BuildTopic() {
            return exportableTopic;
        }
    }
}