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
        private QueryTopicRepository repository;
        private const string EnglishLanguage = "en-GB";
        private const string SpanishLanguage = "es-ES";

        public MkDocsExporter(QueryTopicRepository repository) {
            this.repository = repository;
        }

        public async Task ExportDocumentsForProductToFolder(string productId, List<ProductVersion> versions, DirectoryInfo testDirectory)
        {
            Versions = versions;
            var languages = new[] {SpanishLanguage, EnglishLanguage};
            var topics = await repository.GetEditableTopicsForProduct(productId);
            foreach (var topic in topics)
            {
                await ExportTopic(topic, testDirectory, languages);
            }
        }

        public List<ProductVersion> Versions { get; private set; }

        private async Task ExportTopic(EditableTopic topic, DirectoryInfo directory, params string[] languages)
        {
            foreach (var versionRange in topic.VersionRanges)
            {
                foreach (var productVersion in GetVersionsGroup(versionRange))
                {
                    foreach (var language in languages)
                    {
                        var versionDirectory = directory.CreateSubdirectory(productVersion.VersionName);
                        var languageDirectory = versionDirectory.CreateSubdirectory(language);
                        var documentName = GetDocumentTitleForLanguage(versionRange, language) + ".md";
                        var path = Path.Combine(languageDirectory.FullName, documentName);
                        var documentContent = GetDocumentContentForLanguage(versionRange, language);
                        await WriteTextAsync(path, documentContent);
                    }
                }
            }
        }

        private static string GetDocumentTitleForLanguage(EditableVersionRange versionRange, string language)
        {
            return versionRange.Documents.First(d => d.Language == language).Title;
        }

        private string GetDocumentContentForLanguage(EditableVersionRange versionRange, string language)
        {
            return versionRange.Documents.First(d => d.Language == language).Content;
        }

        private List<ProductVersion> GetVersionsGroup(EditableVersionRange versionRange)
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
