using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business {
    public class ProductRepository {
        private FileRepository FileRepository { get; }

        public ProductRepository(FileRepository fileRepository) {
            FileRepository = fileRepository;
        }

        public List<Product> GetAllProducts() {
            return FileRepository.GetAll<Product>().ToList();
        }
    }
}