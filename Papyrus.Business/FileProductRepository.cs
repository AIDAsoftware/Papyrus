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
            var fileRepo = new FileRepository(Path);
            return fileRepo.GetAll<Product>().ToList();
        }
    }
}