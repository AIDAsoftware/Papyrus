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
        private TopicQueryRepository topicRepository;
        private ProductRepository productRepository;
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
        }

        [TearDown]
        public void DeleteFolder() {
            testDirectory.Delete(true);
        }

        private static string GetContentOf(FileInfo document) {
            return File.ReadAllText(document.FullName);
        }
    }
}