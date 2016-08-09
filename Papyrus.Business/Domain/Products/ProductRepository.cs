using System.Collections.Generic;

namespace Papyrus.Business.Domain.Products {
    public interface ProductRepository {
        List<Product> GetAllProducts();
    }
}