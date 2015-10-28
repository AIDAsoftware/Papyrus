using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private TopicRepository repository;

        public MkDocsExporter(TopicRepository repository) {
            this.repository = repository;
        }

        public async Task ExportDocumentsForProductToFolder(string productId, List<ProductVersion> versions, DirectoryInfo testDirectory)
        {
            Versions = versions;
            var spanishDirectory = "es-ES";
            var englishDirectory = "en-GB";
            var topics = await repository.GetEditableTopicsForProduct(productId);
            foreach (var topic in topics)
            {
                await ExportTopic(topic, testDirectory, spanishDirectory, englishDirectory);
            }
        }

        public List<ProductVersion> Versions { get; private set; }

        private async Task ExportTopic(EditableTopic topic, DirectoryInfo directory, params string[] languages)
        {
            foreach (var editableVersionRange in topic.VersionRanges)
            {
                foreach (var productVersion in GetVersionsGroup(editableVersionRange))
                {
                    foreach (var language in languages)
                    {
                        var versionDirectory = directory.CreateSubdirectory(productVersion.VersionName);
                        var languageDirectory = versionDirectory.CreateSubdirectory(language);
                        var path = ConstructPath(topic, languageDirectory, language);
                        var documentContent = Content(editableVersionRange, language);
                        await WriteTextAsync(path, documentContent);
                    }
                }
            }
        }

        private string Content(EditableVersionRange editableVersionRange, string language)
        {
            return editableVersionRange.Documents.First(d => d.Language == language).Content;
        }

        private string ConstructPath(EditableTopic topic, DirectoryInfo versionDirectory, string language)
        {
            var spanishName = topic.VersionRanges[0].Documents.First(d => d.Language == language).Title + ".md";
            return Path.Combine(versionDirectory.FullName, spanishName);
        }

        private List<ProductVersion> GetVersionsGroup(EditableVersionRange versionRange)
        {
            return AllVersionsContainedIn(versionRange);
        }

        private List<ProductVersion> AllVersionsContainedIn(EditableVersionRange versionRange)
        {
            return Versions.Where(v => versionRange.FromVersion.Release <= v.Release &&
                                       v.Release <= versionRange.ToVersion.Release).ToList();
        }

        private async Task WriteTextAsync(string filePath, string text) {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true)) {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
