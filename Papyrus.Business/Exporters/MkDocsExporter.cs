using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Exporters {
    public class MkDocsExporter {
        private TopicRepository repository;

        public MkDocsExporter(TopicRepository repository) {
            this.repository = repository;
        }

        public async Task ExportDocumentsForProductToFolder(string productId, DirectoryInfo targetDirectory) {
            var spanishDirectory = targetDirectory.CreateSubdirectory("es-ES");
            var englishDirectory = targetDirectory.CreateSubdirectory("en-GB");
            var topics = await repository.GetEditableTopicsForProduct(productId);
            foreach (var topic in topics)
            {
                await ExportTopic(topic, spanishDirectory, englishDirectory);
            }
        }
        
        private async Task ExportTopic(EditableTopic topic, params DirectoryInfo[] languageDirectories)
        {
            foreach (var languageDirectory in languageDirectories)
            {
                var path = ConstructPath(topic, languageDirectory);
                var documentContent = Content(topic, languageDirectory.Name);
                await WriteTextAsync(path, documentContent);
            }
        }

        private static string ConstructPath(EditableTopic topic, DirectoryInfo languageDirectory)
        {
            var spanishName = topic.VersionRanges[0].Documents.First(d => d.Language == languageDirectory.Name).Title + ".md";
            return Path.Combine(languageDirectory.FullName, spanishName);
        }

        private static string Content(EditableTopic topic, string language)
        {
            return topic.VersionRanges[0].Documents.First(d => d.Language == language).Content;
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
