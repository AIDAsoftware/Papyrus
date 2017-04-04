using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories {
    [TestFixture]
    public class SqlProductRepositoryShould : SqlTest {
        private string anyProductId = "AnyProductID";
        private string anyProductName = "AnyProductName";
        private SqlProductRepository sqlProductRepository;

        [SetUp]
        public async void TruncateDataBase() {
            await dbConnection.Execute("TRUNCATE TABLE Product");
            await dbConnection.Execute("TRUNCATE TABLE ProductVersion");
            sqlProductRepository = new SqlProductRepository(dbConnection);
        }

        [Test]
        public async Task load_a_product() {
            const string anyVersionId = "AnyVersionId";
            const string anyVersionName = "AnyVersionName";
            var versions = new List<ProductVersion> { new ProductVersion(anyVersionId, anyVersionName, DateTime.Today) };
            await InsertProduct(new Product(anyProductId, anyProductName, versions));

            var product = await sqlProductRepository.GetProduct(anyProductId);

            product.Id.Should().Be(anyProductId);
            product.Name.Should().Be(anyProductName);
            product.Versions.Count.Should().Be(1);
            var productVersion = product.Versions.First();
            productVersion.VersionId.Should().Be(anyVersionId);
            productVersion.VersionName.Should().Be(anyVersionName);
        }

        [Test, Ignore("Dont run with other tests")]
        public async Task return_null_when_try_to_load_an_no_existing_product()
        {
            var product = await sqlProductRepository.GetProduct("DontExist");

            product.Should().BeNull();
        }


        [Test]
        public async Task load_a_product_with_its_versions() {
            var versions = new List<ProductVersion>
            {
                new ProductVersion("versionId1", "version1", DateTime.Today.AddDays(-1)),
                new ProductVersion("versionId2", "version2", DateTime.Today)
            };
            await InsertProduct(new Product(anyProductId, anyProductName, versions));

            var product = await sqlProductRepository.GetProduct(anyProductId);

            product.Id.Should().Be(anyProductId);
            product.Versions.Should().Contain((version) => version.VersionId.Equals("versionId1"));
            product.Versions.Should().Contain((version) => version.VersionId.Equals("versionId2"));
            product.Versions.Count.Should().Be(2);
        }

        [Test]
	public async Task load_a_list_with_all_products_containing_its_versions() {
            var versionsForPapyrus = new List<ProductVersion>
            {
                new ProductVersion("AnyIdForPapyrus", "AnyVersion", DateTime.Today.AddDays(-1)),
                new ProductVersion("AnotherIdForPapyrus", "AnyVersion", DateTime.Today)
            };
            var versionsForOpportunity = new List<ProductVersion>
            {
                new ProductVersion("AnyIdForOpportunity", "AnyVersion", DateTime.Today)
            };
            var papyrusId = "PapyrusId";
            await InsertProduct(new Product(papyrusId, "Papyrus", versionsForPapyrus));
            var opportunityId = "OpportunityId";
            await InsertProduct(new Product(opportunityId, "Opportunity", versionsForOpportunity));

            var products = await sqlProductRepository.GetAllDisplayableProducts();

            products.Should().Contain(prod => prod.ProductId == "PapyrusId");
            products.Should().Contain(prod => prod.ProductId == "OpportunityId");
            products.ToArray().Length.Should().Be(2);
        }

        private async Task InsertProduct(Product product) {
            await dbConnection.Execute(@"INSERT INTO Product(ProductId, ProductName) 
                                VALUES (@ProductId, @ProductName);",
                                new {
                                    ProductId = product.Id,
                                    ProductName = product.Name,
                                });
            await InsertProductVersions(product);
        }

        private async Task InsertProductVersions(Product product) {
            foreach (var productVersion in product.Versions) {
                await InsertProductVersion(productVersion, product.Id);
            }
        }

        private async Task InsertProductVersion(ProductVersion productVersion, string productId) {
            await dbConnection.Execute(@"INSERT INTO ProductVersion(VersionId, VersionName, ProductId, Release) 
                                VALUES (@VersionId, @VersionName, @ProductId, @Release);",
                new {
                    VersionId = productVersion.VersionId,
                    VersionName = productVersion.VersionName,
                    ProductId = productId,
                    Release = productVersion.Release
                });
        }
    }
}