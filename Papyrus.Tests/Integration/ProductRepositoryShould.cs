using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Papyrus.Business;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Tests.Integration {

    [TestFixture]
    public class ProductRepositoryShould {
        private ProductRepository productRepository;
        private string ProductsPath { get; set; }

        [SetUp]
        public void given_a_products_path() {
            ProductsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Products"));
            productRepository = new FileProductRepository(new FileSystemProvider(ProductsPath));
            Directory.CreateDirectory(ProductsPath);
        }

        [TearDown]
        public void Delete() {
            Directory.Delete(ProductsPath, true);
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
            return new ProductVersion("4321", "0.0.1");
        }

        public void GivenAProductWith(FileProduct product, params ProductVersion[] versions) {
            var productJson = JsonConvert.SerializeObject(product);
            var productPath = Path.Combine(ProductsPath, "1234");
            File.WriteAllText(productPath, productJson);
        }

        private static FileProduct AProductWith(params ProductVersion[] versions) {
            return new FileProduct {
                Name = "Any",
                Id = "Any",
                ProductVersions = versions.ToList()
            };
        }

        private static FileProduct AnyProduct() {
            return AProductWith();
        }
    }
}