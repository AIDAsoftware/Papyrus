using System;
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
                                            FROM [Product] WHERE ProductId = @ProductId;";
            var nameProductColumn = (await connection.Query<string>(selectProductSqlQuery, new { ProductId = productId })).FirstOrDefault();
            var versions = await ProducVersionsForProduct(productId);

            return String.IsNullOrEmpty(nameProductColumn) ? null : new Product(productId, nameProductColumn, versions);
        }

        //TODO: devolver IEnumerable ??

        public async Task<List<DisplayableProduct>> GetAllDisplayableProducts() {
            const string selectProductSqlQuery = @"SELECT ProductId, ProductName
                                            FROM [Product];";

            var productsFromDataBase = (await connection.Query<DisplayableProduct>(selectProductSqlQuery)).ToList();

            return productsFromDataBase;
        }

        public async Task<ProductVersion> GetVersion(string versionId)  // TODO: It is not tested. It is only a try
        {
            const string selectVersionSqlQuery = @"SELECT VersionId, VersionName, Release FROM ProductVersion WHERE VersionId = @VersionId;";

            return (await connection.Query<ProductVersion>(selectVersionSqlQuery, new { VersionId = versionId })).FirstOrDefault();
        }

        public async Task<FullVersionRange> GetFullVersionRangeForProduct(string productId) {
            const string selectFirstVersion = @"SELECT TOP 1 VersionId FROM ProductVersion
                                                WHERE ProductId = @ProductId ORDER BY Release ASC;";
            var firstVersionId = (await connection.Query<string>(selectFirstVersion, new { ProductId = productId })).FirstOrDefault();

            const string selectLatestVersion = @"SELECT TOP 1 VersionId FROM ProductVersion
                                                WHERE ProductId = @ProductId ORDER BY Release DESC;";
            var latestVersionId = (await connection.Query<string>(selectLatestVersion, new { ProductId = productId })).FirstOrDefault();

            return new FullVersionRange(firstVersionId, latestVersionId);
        }

        public async Task<List<ProductVersion>> GetAllVersionsFor(string productId) {
            return (await connection.Query<ProductVersion>(@"SELECT VersionId, VersionName, Release 
                                                            FROM ProductVersion
                                                            WHERE ProductId = @ProductId",
                                                            new { ProductId = productId })).ToList();
        }

        public async Task<ProductVersion> GetLastVersionForProduct(string productId) {
            return (await connection.Query<ProductVersion>(@"SELECT TOP 1 VersionId, VersionName, Release
                                                            FROM ProductVersion
                                                            WHERE ProductId = @ProductId
                                                            ORDER BY Release DESC", new { ProductId = productId })).First();
        }

        public async Task<Product> GetProductForVersions(Product product, List<string> versionsNames) {
            var versions = new List<ProductVersion>();
            foreach (var versionsName in versionsNames) {
                var version = (await connection.Query<ProductVersion>(@"SELECT VersionId, VersionName, Release
                                                                     FROM ProductVersion
                                                                     WHERE ProductId = @ProductId AND VersionName = @VersionName",
                                                                    new {ProductId = product.Id, VersionName = versionsName})).First();
                versions.Add(version);
            }
            return new Product(product.Id, product.Name, versions);
        }

        public async Task<List<string>> GetAllVersionsNamesDistinctingByName() {
            throw new NotImplementedException();
        }

        private async Task<List<ProductVersion>> ProducVersionsForProduct(string productId) {
            const string selectVersionSqlQuery = @"Select VersionId, VersionName, Release
                                            FROM [ProductVersion] WHERE ProductId = @ProductId;";

            return (await connection.Query<ProductVersion>(selectVersionSqlQuery, new { ProductId = productId })).ToList();
        }
    }
}