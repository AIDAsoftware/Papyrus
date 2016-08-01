using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Papyrus.Business {
    public class FileRepository {
        private readonly string directoryPath;

        public FileRepository(string directoryPath) {
            this.directoryPath = directoryPath;
        }

        public IEnumerable<Product> GetAll() {
            var directory = new DirectoryInfo(directoryPath);
            var files = directory.GetFiles();
            return files
                .Select(f => File.ReadAllText(f.FullName))
                .Select(JsonConvert.DeserializeObject<Product>);
        }
    }
}