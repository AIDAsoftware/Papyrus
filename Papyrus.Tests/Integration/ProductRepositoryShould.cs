using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Papyrus.Business;

namespace Papyrus.Tests.Integration {

    [TestFixture]
    public class ProductRepositoryShould {
        private FileProductRepository productRepository;
        private string ProductsPath { get; set; }

        [SetUp]
        public void given_a_products_path() {
            ProductsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Products"));
            productRepository = new FileProductRepository(ProductsPath);
            Directory.CreateDirectory(ProductsPath);
        }

        [TearDown]
        public void Delete() {
            Directory.Delete(ProductsPath, true);
        }

        [Test]
        public void retrieve_products() {
            const string productId = "1234";
            const string productName = "Papyrus";
            GivenAProductWith(id: productId, name: productName);

            var products = productRepository.GetAllProducts();

            products.Should().Contain(p => p.Id ==productId);
            products.Should().Contain(p => p.Name == productName);
        }

        [Test]
        public void retrieve_versions_of_products() {
            var anyVersion = AnyVersion();
            GivenAProductWith(versions: anyVersion);

            var products = productRepository.GetAllProducts();

            products.First().Versions.Should()
                .Contain(v => v.Id == anyVersion.Id && v.Name == anyVersion.Name);
        }

        private static ProductVersion AnyVersion() {
            return new ProductVersion("4321", "0.0.1");
        }

        public void GivenAProductWith(string id = "Any", string name = "Any", params ProductVersion[] versions) {
            var product = new Product(id, name, versions.ToList());
            var productJson = JsonConvert.SerializeObject(product);
            var productPath = Path.Combine(ProductsPath, "1234");
            File.WriteAllText(productPath, productJson);
        }
    }
}