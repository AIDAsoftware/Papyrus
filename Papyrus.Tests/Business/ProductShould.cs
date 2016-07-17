﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class ProductShould
    {
        [Test]
        public void return_its_first_version()
        {
            var firstVersion = new ProductVersion("Any", "Any", DateTime.Now.AddDays(-1));
            var versions = new List<ProductVersion> {
                firstVersion,
                new ProductVersion("Any", "Any", DateTime.Now),
            };
            var product = new Product("AnyId", "AnyName", versions);

            product.FirstVersion().Should().Be(firstVersion);
        }

        [Test]
        public void return_its_last_version()
        {
            var lastVersion = new ProductVersion("Any", "Any", DateTime.Now);
            var versions = new List<ProductVersion> {
                new ProductVersion("Any", "Any", DateTime.Now.AddDays(-1)),
                lastVersion,
            };
            var product = new Product("AnyId", "AnyName", versions);

            product.LastVersion().Should().Be(lastVersion);
        }
    }
}