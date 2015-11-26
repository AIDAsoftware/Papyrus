using System;
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
        private ProductRepository productRepository;
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
            productRepository = Substitute.For<ProductRepository>();
            mkdocsExporter = new MkDocsExporter(topicRepository, productRepository);
        }

        [TearDown]
        public void DeleteFolder() {
            testDirectory.Delete(true);
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