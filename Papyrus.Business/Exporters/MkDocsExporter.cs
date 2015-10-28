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
            targetDirectory.CreateSubdirectory("es-ES");
            targetDirectory.CreateSubdirectory("en-GB");
            var topics = await repository.GetEditableTopicsForProduct(productId);
            foreach (var topic in topics)
            {
                await ExportTopic(topic, targetDirectory);
            }
        }
        
        private async Task ExportTopic(EditableTopic topic, DirectoryInfo targetDirectory)
        {
            var spanishPath = ConstructPath(topic, targetDirectory, "es-ES");
            var spanishContent = Content(topic, "es-ES");
            var englishPath = ConstructPath(topic, targetDirectory, "en-GB");
            var englishContent = Content(topic, "en-GB");
            await WriteTextAsync(spanishPath, spanishContent);
            await WriteTextAsync(englishPath, englishContent);
        }

        private static string ConstructPath(EditableTopic topic, DirectoryInfo targetDirectory, string language)
        {
            var spanishFolder = targetDirectory.GetDirectories().First(d => d.Name == language);
            var spanishName = topic.VersionRanges[0].Documents.First(d => d.Language == language).Title + ".md";
            return Path.Combine(spanishFolder.FullName, spanishName);
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
