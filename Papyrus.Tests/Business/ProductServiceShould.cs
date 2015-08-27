using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Products.Exceptions;

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
                Task.FromResult(new Product(id: AnyId, name: AnyProductName))
            );
            service = new ProductService(repository);
        }

        [Test]
        public async Task save_a_product_when_it_is_created() {
            var product = new Product(name: AnyProductName).WithDescription("Any product description");

            await service.Create(product);

            repository.Received().Save(product);
            repository.Received().Save(Arg.Is<Product>(x => !string.IsNullOrWhiteSpace(x.Id)));
        }

        [Test]
        public async void get_a_saved_product_when_it_is_requested() {
            var id = "1";
            repository.GetProduct(id).Returns(Task.FromResult(new Product(id: id, name: AnyProductName))
            );

            var product = await service.GetProductById(id);

            product.Id.Should().Be(id);
        }


        [Test]
        public async void update_a_given_product_when_it_is_modified() {
            var product = new Product(id: AnyId, name: AnyProductName);
            await service.Update(product);

            repository.Received().Update(product);
        }

        [Test]
        [ExpectedException(typeof(ProductNotFoundException))]
        public async Task throw_an_exception_when_try_to_update_a_non_existent_product() {
            var product = new Product(id: "NoExistingId", name: AnyProductName);
            await service.Update(product);
        }


        [Test]
        public async Task remove_a_given_product_when_it_is_deleted() {
            const string productId = AnyId;
            await service.Remove(productId);
            repository.Received().Delete(productId);
        }

        [Test]
        [ExpectedException(typeof(ProductNotFoundException))]
        public async Task throw_an_exception_when_try_to_delete_a_no_existing_product() {
            await service.Remove("NoExistingId");
        }


        [Test]
        public async Task return_a_list_of_products_when_user_want_to_see_all_products() {
            repository.GetAllProducts().Returns(Task.FromResult(new List<Product> {
                new Product(id: AnyId, name: AnyProductName),
            }));

            var products = await service.AllProducts();

            products.Should().Contain(x => x.Id == AnyId);
            products.Length.Should().Be(1);
        }

    }
}
