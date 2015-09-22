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
        [SetUp]
        public async void TruncateDataBase()
        {
            await dbConnection.Execute("TRUNCATE TABLE ProductVersion");
        }

        [Test]
        public async void load_a_product()
        {
            await InsertProductVersionWith(
                productId: "AnyProductID", versionId:"1", productName: "AnyProductName", versionName:"AnyVersionName"
            );

            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyProductID");

            product.Id.Should().Be("AnyProductID");
            product.Versions.First().VersionId.Should().Be("1");
            product.Versions.First().VersionName.Should().Be("AnyVersionName");
            product.Versions.First().ProductName.Should().Be("AnyProductName");
            product.Versions.Count.Should().Be(1);
        }

        [Test]
        public async void load_a_product_with_its_versions()
        {
            await InsertProductVersionWith(
                productId: "AnyProductID", versionId:"1", productName: "AnyProductName", versionName:"AnyVersionName"
            );
            await InsertProductVersionWith(
                productId: "AnyProductID", versionId:"2", productName: "AnyProductName", versionName:"AnotherVersionName"
            );

            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyProductID");

            product.Id.Should().Be("AnyProductID");
            product.Versions.Should().Contain((version) => version.VersionId.Equals("1"));
            product.Versions.Should().Contain((version) => version.VersionId.Equals("2"));
            product.Versions.Count.Should().Be(2);
        }

        [Test]
        public async void return_null_when_try_to_load_an_no_existing_product()
        {
            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyId");

            product.Should().Be(null);
        }

        [Test]
        public async Task load_a_list_with_all_products()
        {
            await InsertProductVersionWith(productId: "1", versionId: "1", productName: "anyProductName", versionName: "anyVersionName");
            await InsertProductVersionWith(productId: "2", versionId: "1", productName: "anyProductName", versionName: "anyVersionName");

            var products = await new SqlProductRepository(dbConnection).GetAllProducts();

            products.Should().Contain(prod => prod.Id == "1");
            products.Should().Contain(prod => prod.Id == "2");
            products.ToArray().Length.Should().Be(2);
        }

        private async Task InsertProductVersionWith(string productId, string versionId, string productName, string versionName)
        {
            await dbConnection.Execute(@"INSERT ProductVersion(ProductId, VersionId, ProductName, VersionName) 
                                VALUES (@ProductId, @VersionId, @ProductName, @VersionName);",
                                new {
                                    ProductId = productId,
                                    VersionId = versionId,
                                    ProductName = productName,
                                    VersionName = versionName,
                                });
        }
    }
}