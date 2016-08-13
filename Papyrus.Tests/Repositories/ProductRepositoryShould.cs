using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Papyrus.Business.Domain.Products;
using Papyrus.Infrastructure.Core;
using Papyrus.Infrastructure.Repositories;

namespace Papyrus.Tests.Repositories {

    [TestFixture]
    public class ProductRepositoryShould : FileRepositoryTest {
        private ProductRepository productRepository;

        [SetUp]
        public void SetUp() {
            productRepository = new FileProductRepository(new JsonFileSystemProvider(WorkingDirectoryPath));
            Directory.CreateDirectory(WorkingDirectoryPath);
        }

        [Test]
        public void retrieve_products() {
            var anyProduct = AnyProduct();
            GivenAProductWith(anyProduct);

            var products = productRepository.GetAllProducts();

            products.First().Id.Should().Be(anyProduct.Id);
            products.First().Name.Should().Be(anyProduct.Name);
        }

        [Test]
        public void retrieve_versions_of_products() {
            var anyVersion = AnyVersion();
            GivenAProductWith(AProductWith(anyVersion));

            var products = productRepository.GetAllProducts();

            products.First().Versions.Should()
                .Contain(v => v.Id == anyVersion.Id && v.Name == anyVersion.Name);
        }

        private static ProductVersion AnyVersion() {
            return new ProductVersion(AnyUniqueString(), AnyUniqueString());
        }

        public void GivenAProductWith(SerializableProduct product, params ProductVersion[] versions) {
            var productJson = JsonConvert.SerializeObject(product);
            var productPath = Path.Combine(WorkingDirectoryPath, AnyUniqueString());
            File.WriteAllText(productPath, productJson);
        }

        private static SerializableProduct AProductWith(params ProductVersion[] versions) {
            return new SerializableProduct {
                Name = AnyUniqueString(),
                Id = AnyUniqueString(),
                ProductVersions = versions.ToList()
            };
        }

        private static SerializableProduct AnyProduct() {
            return AProductWith();
        }
    }
}