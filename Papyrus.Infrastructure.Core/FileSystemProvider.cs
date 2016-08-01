using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Papyrus.Business;

namespace Papyrus.Infrastructure.Core {
    public class FileSystemProvider {
        public readonly string DirectoryPath;

        public FileSystemProvider(string directoryPath) {
            this.DirectoryPath = directoryPath;
        }

        public IEnumerable<T> GetAll<T>() where T : SerializableItem {
            var directory = new DirectoryInfo(DirectoryPath);
            var files = directory.GetFiles();
            return files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<T>);
        }

        public void Create(SerializableItem serializableItem) {
            Directory.CreateDirectory(DirectoryPath);
            var documentPath = Path.Combine(DirectoryPath, serializableItem.Id);
            var jsonDocument = JsonConvert.SerializeObject(serializableItem);
            File.WriteAllText(documentPath, jsonDocument);
        }
    }
}