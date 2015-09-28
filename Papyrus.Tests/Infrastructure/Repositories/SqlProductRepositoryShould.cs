using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories
{
    [TestFixture]
    public class SqlProductRepositoryShould : SqlTest
    {
        private string anyProductId = "AnyProductID";
        private string anyProductName = "AnyProductName";

        [SetUp]
        public async void TruncateDataBase()
        {
            await dbConnection.Execute("TRUNCATE TABLE Product");
            await dbConnection.Execute("TRUNCATE TABLE ProductVersion");
        }

        [Test]
        public async void load_a_product()
        {
            const string anyVersionId = "AnyVersionId";
            const string anyVersionName = "AnyVersionName";
            var versions = new List<ProductVersion> { new ProductVersion(anyVersionId, anyVersionName) };
            await InsertProduct(new Product(anyProductId, anyProductName, versions));

            var product = await new SqlProductRepository(dbConnection).GetProduct(anyProductId);

            product.Id.Should().Be(anyProductId);
            product.Name.Should().Be(anyProductName);
            product.Versions.Count.Should().Be(1);
            var productVersion = product.Versions.First();
            productVersion.VersionId.Should().Be(anyVersionId);
            productVersion.VersionName.Should().Be(anyVersionName);
        }

        [Test]
        public async void load_a_product_with_its_versions()
        {
            var versions = new List<ProductVersion>
            {
                new ProductVersion("versionId1", "version1"),
                new ProductVersion("versionId2", "version2")
            };
            await InsertProduct(new Product(anyProductId, anyProductName, versions));

            var product = await new SqlProductRepository(dbConnection).GetProduct(anyProductId);

            product.Id.Should().Be(anyProductId);
            product.Versions.Should().Contain((version) => version.VersionId.Equals("versionId1"));
            product.Versions.Should().Contain((version) => version.VersionId.Equals("versionId2"));
            product.Versions.Count.Should().Be(2);
        }

        [Test]
        public async void return_null_when_try_to_load_an_no_existing_product()
        {
            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyId");

            product.Should().Be(null);
        }

        [Test]
        public async Task load_a_list_with_all_products_containing_its_versions()
        {
            var versionsForPapyrus = new List<ProductVersion>
            {
                new ProductVersion("AnyIdForPapyrus", "AnyVersion"),
                new ProductVersion("AnotherIdForPapyrus", "AnyVersion")
            };
            var versionsForOpportunity = new List<ProductVersion>
            {
                new ProductVersion("AnyIdForOpportunity", "AnyVersion")
            };
            var papyrusId = "PapyrusId";
            await InsertProduct(new Product(papyrusId, "Papyrus", versionsForPapyrus));
            var opportunityId = "OpportunityId";
            await InsertProduct(new Product(opportunityId, "Opportunity", versionsForOpportunity));

            var products = await new SqlProductRepository(dbConnection).GetAllProducts();

            products.First(prod => prod.Id == papyrusId).Versions.Count.Should().Be(2);
            products.First(prod => prod.Id == opportunityId).Versions.Count.Should().Be(1);
            products.ToArray().Length.Should().Be(2);
        }

        private async Task InsertProduct(Product product)
        {
            await dbConnection.Execute(@"INSERT Product(ProductId, ProductName) 
                                VALUES (@ProductId, @ProductName);",
                                new {
                                    ProductId = product.Id,
                                    ProductName = product.Name,
                                });
            await InsertProductVersions(product);
        }

        private async Task InsertProductVersions(Product product)
        {
            foreach (var productVersion in product.Versions)
            {
                await dbConnection.Execute(@"INSERT ProductVersion(VersionId, VersionName, Product) 
                                VALUES (@VersionId, @VersionName, @Product);",
                    new
                    {
                        VersionId = productVersion.VersionId,
                        VersionName = productVersion.VersionName,
                        Product = product.Id
                    });
            }
        }
    }
}