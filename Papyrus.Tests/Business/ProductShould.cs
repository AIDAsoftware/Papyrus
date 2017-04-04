using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class ProductShould
    {
        private readonly DateTime today = DateTime.Today;
        private readonly DateTime yesterday = DateTime.Today.AddDays(-1);

        [Test]
        public void return_its_first_version()
        {
            var product = ProductWith(
                Versions(AVersionFrom(yesterday), AVersionFrom(today)));

            product.FirstVersion().Should().Be(AVersionFrom(yesterday));
        }

        [Test]
        public void return_its_last_version()
        {
            var product = ProductWith(
                Versions(AVersionFrom(yesterday), AVersionFrom(today)));

            product.LastVersion().Should().Be(AVersionFrom(today));
        }

        private Product ProductWith(List<ProductVersion> versions)
        {
            return new Product("AnyId", "AnyName", versions);
        }

        private List<ProductVersion> Versions(params ProductVersion[] versions)
        {
            return versions.ToList();
        }


        private static ProductVersion AVersionFrom(DateTime dateTime)
        {
            return new ProductVersion("Any", "Any", dateTime);
        }

    }
}