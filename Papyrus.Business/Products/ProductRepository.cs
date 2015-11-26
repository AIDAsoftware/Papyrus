using Papyrus.Business.Exporters;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ProductRepository
    {
        Task<Product> GetProduct(string productId);
        Task<List<DisplayableProduct>> GetAllDisplayableProducts();
        Task<ProductVersion> GetVersion(string versionId);
        Task<FullVersionRange> GetFullVersionRangeForProduct(string productId);
        Task<List<ProductVersion>> GetAllVersionsFor(string productId);
        Task<ProductVersion> GetLastVersionForProduct(string productId);
    }
}