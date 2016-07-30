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
        private string ProductsPath { get; set; }

        [SetUp]
        public void given_a_products_path() {
            ProductsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Products"));
            Directory.CreateDirectory(ProductsPath);
        }

        [TearDown]
        public void Delete() {
            Directory.Delete(ProductsPath, true);
        }

        [Test]
        public void retrieve_all_products() {
            var product = new Product("1234", "Papyrus", new List<ProductVersion> {
                new ProductVersion("4321", "0.0.1")
            });
            var productJson = JsonConvert.SerializeObject(product);
            var productPath = Path.Combine(ProductsPath, "1234");
            File.WriteAllText(productPath, productJson);
            var productRepository = new FileProductRepository(ProductsPath);

            var products = productRepository.GetAllProducts();

            products.Should().Contain(p => p.Id == product.Id);
            products.First().Versions.Should().Contain(v => v.Id == "4321" && v.Name == "0.0.1");
        }
    }
}