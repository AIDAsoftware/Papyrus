using Papyrus.Business.Documents;

namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ProductRepository
    {
        Task<Product> GetProduct(string productId);
        Task<List<DisplayableProduct>> GetAllDisplayableProducts();
    }
}