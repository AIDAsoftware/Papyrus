using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business {
    public class FileProductRepository {
        private FileRepository FileRepository { get; }

        public FileProductRepository(FileRepository fileRepository) {
            FileRepository = fileRepository;
        }

        public List<Product> GetAllProducts() {
            return FileRepository.GetAll<Product>().ToList();
        }
    }
}