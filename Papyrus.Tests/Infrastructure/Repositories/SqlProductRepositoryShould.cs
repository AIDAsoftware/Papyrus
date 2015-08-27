
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
            await dbConnection.Execute("TRUNCATE TABLE Products");
        }

        [Test]
        public async Task save_a_product() {
            var product = new Product("AnyId", "AnyName");

            await new SqlProductRepository(dbConnection).Save(product);

            var requestedProducts = await LoadProductWithId("AnyId");
            requestedProducts.ShouldBeEquivalentTo(product);
        }

        [Test]
        public async void load_a_product()
        {
            await InsertProductWith(
                id: "AnyID", name: "AnyName", description: "AnyDescription"
            );

            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyId");

            product.Id.Should().Be("AnyID");
            product.Name.Should().Be("AnyName");
            product.Description.Should().Be("AnyDescription");
        }

        [Test]
        public async void return_null_when_try_to_load_an_no_existing_product()
        {
            var product = await new SqlProductRepository(dbConnection).GetProduct("AnyId");

            product.Should().Be(null);
        }

        [Test]
        public async Task update_a_product()
        {
            await dbConnection.Execute(@"INSERT Products(Id, Name) 
                                VALUES (@id, @name);",
                                new { id = "AnyId", name = "AnyName" });

            var product = new Product("AnyId", "AnyName").WithDescription("AnyDescription");

            await new SqlProductRepository(dbConnection).Update(product);
            var updatedProduct = await LoadProductWithId("AnyId");

            updatedProduct.ShouldBeEquivalentTo(product);
        }

        [Test]
        public async void remove_a_product()
        {
            const string id = "AnyId";
            await InsertProductWith(id: id, name: "AnyName");
            await new SqlProductRepository(dbConnection).Delete(id);

            var product = await LoadProductWithId(id);

            product.Should().BeNull();
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

        private async Task<Product> LoadProductWithId(string id)
        {
            var result = await dbConnection
                .Query<Product>(@"SELECT Id, Name, Description " +
                                 "FROM [Products]" +
                                 "WHERE Id = @Id;", new { Id = id });
            return result.FirstOrDefault();
        }
    }
}