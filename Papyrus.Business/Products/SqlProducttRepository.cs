using System.Runtime.CompilerServices;

namespace Papyrus.Business.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Infrastructure.Core.Database;

    public class SqlProductRepository : ProductRepository
    {
        private readonly DatabaseConnection connection;

        public SqlProductRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task<Product> GetProduct(string productId)
        {
            const string selectSqlQuery = @"SELECT ProductId, VersionId, ProductName, ProductId
                                            FROM [ProductVersion] WHERE ProductId = @ProductId;";
            var allVersionsForCurrentProduct = (await connection.Query<dynamic>(selectSqlQuery, new { ProductId = productId })).ToList();

            if (!allVersionsForCurrentProduct.Any()) return null;

            var versions = ExtractOnlyVersionsFrom(allVersionsForCurrentProduct);

            return new Product(productId, versions);
        }

        private static List<ProductVersion> ExtractOnlyVersionsFrom(List<dynamic> allVersionsForCurrentProduct)
        {
            var versions = new List<ProductVersion>();

            allVersionsForCurrentProduct.ForEach(version =>
                versions.Add(new ProductVersion(version.VersionId, version.VersionName, version.ProductName)));

            return versions;
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Product>> GetAllProducts()
        {
            const string selectAllProductsSqlQuery = @"SELECT ProductId, VersionId, ProductName, ProductId
                                                        FROM ProductVersion";
            var allProductVersions = (await connection.Query<dynamic>(selectAllProductsSqlQuery)).ToList();
                  
            var products = new List<Product>();
                              
            allProductVersions.ForEach((productVersion) =>
            {
                var versions = new List<ProductVersion> { new ProductVersion(productVersion.VersionId, productVersion.VersionName, productVersion.ProductName) };
                products.Add(new Product(productVersion.ProductId, versions));
            });

            return products;
        }
    }
}