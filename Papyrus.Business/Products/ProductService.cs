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

        public virtual async Task Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Id))
                throw new ProductIdMustBeDefinedException();
            await repository.Save(product);
        }

        public virtual async Task<Product> GetProductById(string id)
        {
            return await repository.GetProduct(id);
        }

        public virtual async Task Update(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Id))
                throw new ProductIdMustBeDefinedException();
            if (await GetProductById(product.Id) == null)
                throw new ProductNotFoundException();
            await repository.Update(product);
        }

        public virtual async Task Remove(string productId)
        {
            if (await GetProductById(productId) == null)
                throw new ProductNotFoundException();
            await repository.Delete(productId);
        }

        public virtual async Task<Product[]> AllProducts()
        {
            return (await repository.GetAllProducts()).ToArray();
        }
    }
}