using System.Collections.Generic;
using System.Linq;
using Papyrus.Business;

namespace Papyrus.Infrastructure.Core {
    public class FileProductRepository : ProductRepository {
        private FileRepository FileRepository { get; }

        public FileProductRepository(FileRepository fileRepository) {
            FileRepository = fileRepository;
        }

        public List<Product> GetAllProducts() {
            return FileRepository.GetAll<FileProduct>()
                .Select(p => new Product(p.Id, p.Name, p.ProductVersions))
                .ToList();
        }
    }
}