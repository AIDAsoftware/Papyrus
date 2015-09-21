namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ProductRepository
    {
        Task<Product> GetProduct(string id);
        Task<List<Product>> GetAllProducts();
    }
}