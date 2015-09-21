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
            await dbConnection.Execute("TRUNCATE TABLE Products");
        }

        [Test]
        public async void load_a_product()
        {
            await InsertProductWith(
                id: "AnyID", name: "AnyName", description: "AnyDescription"
            );

            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyId");

            product.Id.Should().Be("AnyID");

            product.Description.Should().Be("AnyDescription");
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
            await InsertProductWith(id: "1", name: "anyProductName");
            await InsertProductWith(id: "2", name: "anyOtherProductName");

            var products = await new SqlProductRepository(dbConnection).GetAllProducts();

            products.Should().Contain(prod => prod.Id == "1");
            products.Should().Contain(prod => prod.Id == "2");
            products.ToArray().Length.Should().Be(2);
        }


        private async Task InsertProductWith(string id, string name, string description = null)
        {
            await dbConnection.Execute(@"INSERT Products(Id, Name, Description) 
                                VALUES (@Id, @Name, @Description);",
                                new {
                                    Id = id,
                                    Name = name,
                                    Description = description
                                });
        }
    }
}