using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Papyrus.Infrastructure.Core {
    //TODO : SRP?
    public class JsonFileSystemProvider {
        public readonly string DirectoryPath;

        public JsonFileSystemProvider(string directoryPath) {
            DirectoryPath = directoryPath;
        }

        public IEnumerable<T> GetAll<T>() where T : SerializableItem {
            var directory = new DirectoryInfo(DirectoryPath);
            var files = directory.GetFiles();
            return files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<T>);
        }

        public void Persist(SerializableItem serializableItem) {
            Directory.CreateDirectory(DirectoryPath);
            var documentPath = Path.Combine(DirectoryPath, serializableItem.Id);
            var jsonDocument = JsonConvert.SerializeObject(serializableItem);
            File.WriteAllText(documentPath, jsonDocument);
        }
    }
}