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
            var documentPath = Path.Combine(languageDirectory.FullName, documentName);
            await WriteContentIn(documentPath);
        }

        private async Task WriteContentIn(string filePath) {
            await FileWriter.WriteFileWithContent(filePath, Content);
        }
    }
}