using System;
using Papyrus.Business.Documents;
using Papyrus.Business.Exporters;
using Papyrus.Business.Topics;

namespace Papyrus.Business.Products {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Infrastructure.Core.Database;

    public class SqlProductRepository : ProductRepository {
        private readonly DatabaseConnection connection;

        public SqlProductRepository(DatabaseConnection connection) {
            this.connection = connection;
        }

        public async Task<Product> GetProduct(string productId) {
            const string selectProductSqlQuery = @"SELECT ProductName
                                            FROM Product WHERE ProductId = @ProductId;";
            var nameProductColumn = (await connection.Query<string>(selectProductSqlQuery, new { ProductId = productId })).FirstOrDefault();
            var versions = await ProducVersionsForProduct(productId);

            return string.IsNullOrEmpty(nameProductColumn) ? null : new Product(productId, nameProductColumn, versions);
        }

        //TODO: devolver IEnumerable ??

        public async Task<List<DisplayableProduct>> GetAllDisplayableProducts() {
            const string selectProductSqlQuery = @"SELECT ProductId, ProductName
                                            FROM [Product];";

            var productsFromDataBase = (await connection.Query<DisplayableProduct>(selectProductSqlQuery)).ToList();

            return productsFromDataBase;
        }

        private async Task<List<ProductVersion>> ProducVersionsForProduct(string productId) {
            const string selectVersionSqlQuery = @"Select VersionId, VersionName, Release
                                            FROM [ProductVersion] WHERE ProductId = @ProductId;";

            return (await connection.Query<ProductVersion>(selectVersionSqlQuery, new { ProductId = productId })).ToList();
        }
    }
}