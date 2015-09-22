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
            const string selectSqlQuery = @"SELECT ProductId, VersionId, ProductName, ProductId, Description
                                            FROM [ProductVersion] WHERE ProductId = @ProductId;";
            var queryResult = (await connection.Query<dynamic>(selectSqlQuery, new { ProductId = productId })).FirstOrDefault();

            if (queryResult == null) return null;

            var productVersions = new List<ProductVersion>
            {
                new ProductVersion(queryResult.VersionId, queryResult.VersionName, queryResult.ProductName)
            };
            return new Product(queryResult.ProductId, productVersions, queryResult.Description);
        }

        //TODO: devolver IEnumerable ??
        public async Task<List<Product>> GetAllProducts()
        {
            const string selectAllProductsSqlQuery = @"SELECT ProductId, VersionId, ProductName, ProductId, Description
                                                        FROM ProductVersion";
            var allProductVersions = (await connection.Query<dynamic>(selectAllProductsSqlQuery)).ToList();
                  
            var products = new List<Product>();
                              
            allProductVersions.ForEach((productVersion) =>
            {
                var versions = new List<ProductVersion> { new ProductVersion(productVersion.VersionId, productVersion.VersionName, productVersion.ProductName) };
                products.Add(new Product(productVersion.ProductId, versions, productVersion.Description));
            });

            return products;
        }
    }
}