using System;
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
            const string selectProductSqlQuery = @"SELECT ProductName
                                            FROM [Product] WHERE ProductId = @ProductId;";
            var nameProductColumn = (await connection.Query<string>(selectProductSqlQuery, new { ProductId = productId })).FirstOrDefault();
            var versions = await ProducVersionsForProduct(productId);

            return String.IsNullOrEmpty(nameProductColumn) ? null : new Product(productId, nameProductColumn, versions);
        }

        //TODO: devolver IEnumerable ??

        public async Task<List<Product>> GetAllProducts()
        {
            const string selectProductSqlQuery = @"SELECT ProductId, ProductName
                                            FROM [Product];";

            var productsFromDataBase = (await connection.Query<dynamic>(selectProductSqlQuery)).ToList();

            var products = new List<Product>();

            foreach (var product in productsFromDataBase)
            {
                var versionsForProduct = await ProducVersionsForProduct(product.ProductId);
                products.Add(new Product(product.ProductId, product.ProductName, versionsForProduct));
            }

            return products;
        }

        public async Task<string> GetVersion(string versionId)  // TODO: It is not tested. It is only a try
        {
            const string selectVersionSqlQuery = @"SELECT VersionName FROM ProductVersion WHERE VersionId = @VersionId;";

            return (await connection.Query<string>(selectVersionSqlQuery, new { VersionId = versionId })).FirstOrDefault();
        }

        private async Task<List<ProductVersion>> ProducVersionsForProduct(string productId)
        {
            const string selectVersionSqlQuery = @"Select VersionId, VersionName
                                            FROM [ProductVersion] WHERE Product = @ProductId;";

            return (await connection.Query<ProductVersion>(selectVersionSqlQuery, new {ProductId = productId})).ToList();
        }
    }
}