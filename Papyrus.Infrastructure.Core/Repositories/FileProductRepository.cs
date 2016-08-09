using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Domain.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Infrastructure.Repositories {
    public class FileProductRepository : ProductRepository {
        private JsonFileSystemProvider JsonFileSystemProvider { get; }

        public FileProductRepository(JsonFileSystemProvider jsonFileSystemProvider) {
            JsonFileSystemProvider = jsonFileSystemProvider;
        }

        public List<Product> GetAllProducts() {
            return JsonFileSystemProvider.GetAll<SerializableProduct>()
                .Select(p => new Product(p.Id, p.Name, p.ProductVersions))
                .ToList();
        }
    }
}