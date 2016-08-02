using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Tests.Infrastructure {
    [TestFixture]
    public class FileSystemProviderShould {
        private readonly string directoryToPersist = 
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Items"));
        private FileSystemProvider provider;

        [SetUp]
        public void SetUp() {
            provider = new FileSystemProvider(directoryToPersist);
        }

        [Test]
        public void insert_an_item() {
            var expectedItem = AnyItem();

            provider.Create(expectedItem);

            GetItemWithId(expectedItem.Id).ShouldBeEquivalentTo(expectedItem);
        }

        private TestSerializableItem GetItemWithId(string id) {
            return JsonConvert.DeserializeObject<TestSerializableItem>(GetContentOfFileNamed(id));
        }

        private string GetContentOfFileNamed(string fileName) {
            return new DirectoryInfo(directoryToPersist)
                            .GetFiles(fileName)
                            .Select(f => File.ReadAllText(f.FullName))
                            .First();
        }

        private static TestSerializableItem AnyItem() {
            return new TestSerializableItem {Id = AnyUniqueString(), Text = AnyUniqueString()};
        }

        private static string AnyUniqueString() {
            return Guid.NewGuid().ToString();
        }

        private class TestSerializableItem : SerializableItem {
            public string Id { get; set; }
            public string Text { get; set; }
        }
    }
}