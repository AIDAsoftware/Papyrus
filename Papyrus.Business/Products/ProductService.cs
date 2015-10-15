namespace Papyrus.Business.Products
{
    using System.Threading.Tasks;
    using Exceptions;

    public class ProductService
    {
        private readonly ProductRepository repository;

        public ProductService(ProductRepository repository)
        {
            this.repository = repository;
        }

        public virtual async Task<Product> GetProductById(string id)
        {
            return await repository.GetProduct(id);
        }
    }
}