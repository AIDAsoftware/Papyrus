using System.Collections.Generic;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Business.Actions {
    public class GetProducts {
        private readonly ProductRepository productsRepository;

        public GetProducts(ProductRepository productsRepository) {
            this.productsRepository = productsRepository;
        }

        public IReadOnlyCollection<Product> Execute() {
            return productsRepository.GetAllProducts();
        }
    }
}