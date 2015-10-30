using System.IO;
using System.Text;
using System.Threading.Tasks;
using Papyrus.Business.Exporters;

namespace Papyrus.Business.Topics {
    public class ExportableDocument {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Language { get; private set; }

        public ExportableDocument(string title, string content, string language) {
            Title = title;
            Content = content;
            Language = language;
        }

        public async Task ExportDocument(DirectoryInfo languageDirectory, string extension) {
            var documentName = Title + extension;
            var path = Path.Combine(languageDirectory.FullName, documentName);
            await WriteTextAsync(path, Content);
        }

        private static async Task WriteTextAsync(string filePath, string text) {
            var encodedText = Encoding.UTF8.GetBytes(text);

            using (var sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true)) {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}