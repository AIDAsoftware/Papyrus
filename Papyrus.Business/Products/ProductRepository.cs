namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ProductRepository
    {
        Task Save(Product product);
        Task<Product> GetProduct(string id);
        Task Update(Product product);
        Task Delete(string productId);
        Task<List<Product>> GetAllProducts();
    }
}