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
        [Test]
        public void get_all_products() {
            var version = new ProductVersion("any", "any");
            var product = new Product("any", "any", new List<ProductVersion> {version});
            var productsRepository = Substitute.For<ProductRepository>();
            productsRepository.GetAllProducts().Returns(new List<Product> {product});

            var getAllProducts = new GetProducts(productsRepository);
            var products = getAllProducts.Execute();

            products.Single().Should().Be(product);
        }
    }
}