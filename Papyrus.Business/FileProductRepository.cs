using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Papyrus.Business {
    public class FileProductRepository {
        public string Path { get; }

        public FileProductRepository(string path) {
            Path = path;
        }

        public List<Product> GetAllProducts() {
            var directory = new DirectoryInfo(Path);
            var files = directory.GetFiles();
            return files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<Product>)
                .ToList();
        }
    }
}