using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Actions;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Tests.Business {
    [TestFixture]
    public class ProductActionsTests {
        private ProductRepository productRepository;

        [SetUp]
        public void SetUp() {
            productRepository = Substitute.For<ProductRepository>();
        }

        [Test]
        public void get_all_products() {
            var product = AnyProduct();
            productRepository.GetAllProducts().Returns(AListWith(product));

            var products = ExecuteGetAllProducts();

            products.Single().Should().Be(product);
        }

        private IReadOnlyCollection<Product> ExecuteGetAllProducts() {
            var getAllProducts = new GetProducts(productRepository);
            var products = getAllProducts.Execute();
            return products;
        }

        private static Product AnyProduct() {
            return new Product("any", "any", new List<ProductVersion> {AnyVersion()});
        }

        private static ProductVersion AnyVersion() {
            return new ProductVersion("any", "any");
        }

        private static List<Product> AListWith(Product product) {
            return new List<Product> {product};
        }
    }
}