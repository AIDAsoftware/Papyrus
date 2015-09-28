namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ProductRepository
    {
        Task<Product> GetProduct(string productId);
        Task<List<Product>> GetAllProducts();
        Task<ProductVersion> GetVersion(string versionId);
    }
}