using System.Net;
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
            const string selectSqlQuery = @"SELECT ProductId, VersionId, ProductName, VersionName
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
            const string selectAllProductsSqlQuery = @"SELECT VersionId, ProductName, ProductId, VersionName
                                                        FROM ProductVersion";
            var allProductVersions = (await connection.Query<dynamic>(selectAllProductsSqlQuery)).ToList();

            return ProductsFrom(allProductVersions).ToList();
        }

        private static IEnumerable<Product> ProductsFrom(List<dynamic> allProductVersions)
        {
            var products = new List<Product>();
            var groupedProducts = allProductVersions.GroupBy(x => x.ProductId);

            foreach (var productGroup in groupedProducts) {
                var versions =
                    productGroup.Select(pv => new ProductVersion(pv.VersionId, pv.VersionName, pv.ProductName));
                products.Add(new Product(productGroup.Key, versions.ToList()));

            }
            return products;
        }

        private static List<dynamic> FilterToGetOnlyCurrentProductVersions(IEnumerable<dynamic> allProductVersions, dynamic productVersion)
        {
            return allProductVersions
                .Where((prod) => prod.ProductId.Equals(productVersion.ProductId))
                .ToList();
        }
    }
}