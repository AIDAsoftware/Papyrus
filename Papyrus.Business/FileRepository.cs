using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Papyrus.Business {
    public class FileRepository {
        public readonly string DirectoryPath;

        public FileRepository(string directoryPath) {
            this.DirectoryPath = directoryPath;
        }

        public IEnumerable<T> GetAll<T>() {
            var directory = new DirectoryInfo(DirectoryPath);
            var files = directory.GetFiles();
            return files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<T>);
        }

        public void CreateFile(FileDocument fileDocument) {
            Directory.CreateDirectory(DirectoryPath);
            var documentPath = Path.Combine(DirectoryPath, fileDocument.Id);
            var jsonDocument = JsonConvert.SerializeObject(fileDocument);
            File.WriteAllText(documentPath, jsonDocument);
        }
    }
}