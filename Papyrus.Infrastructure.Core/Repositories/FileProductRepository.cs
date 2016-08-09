using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Domain.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Infrastructure.Repositories {
    public class FileProductRepository : ProductRepository {
        private FileSystemProvider FileSystemProvider { get; }

        public FileProductRepository(FileSystemProvider fileSystemProvider) {
            FileSystemProvider = fileSystemProvider;
        }

        public List<Product> GetAllProducts() {
            return FileSystemProvider.GetAll<FileProduct>()
                .Select(p => new Product(p.Id, p.Name, p.ProductVersions))
                .ToList();
        }
    }
}