using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Papyrus.Business.Exporters {
    public static class FileWriter {
        public static async Task WriteFileWithContent(string filePath, string content) {
            var encodedText = Encoding.UTF8.GetBytes(content ?? "");

            using (var sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true)) {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}