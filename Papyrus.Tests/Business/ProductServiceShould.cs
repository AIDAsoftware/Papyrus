using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Products;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class ProductServiceShould {
        private const string AnyId = "AnyId";
        private const string AnyProductName = "AnyProductName";
        private ProductRepository repository;
        private ProductService service;

        [SetUp]
        public void SetUp() {
            repository = Substitute.For<ProductRepository>();
            repository.GetProduct(AnyId).Returns(
                Task.FromResult(new Product())
                //Task.FromResult(new Product(id: AnyId, name: AnyProductName))
            );
            service = new ProductService(repository);
        }

        [Test]
        public async void get_a_saved_product_when_it_is_requested() {
            var id = "1";
            repository.GetProduct(id).Returns(Task.FromResult(new Product("1")));
            //repository.GetProduct(id).Returns(Task.FromResult(new Product(id: id, name: AnyProductName)));

            var product = await service.GetProductById(id);

            product.Id.Should().Be(id);
        }

        [Test]
        public async Task return_a_list_of_products_when_user_want_to_see_all_products() {
            repository.GetAllProducts().Returns(Task.FromResult(new List<Product> {
                new Product(id: AnyId),
            }));

            var products = await service.AllProducts();

            products.Should().Contain(x => x.Id == AnyId);
            products.Length.Should().Be(1);
        }

    }
}
