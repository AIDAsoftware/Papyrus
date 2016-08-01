using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business {
    public interface ProductRepository {
        List<Product> GetAllProducts();
    }
}