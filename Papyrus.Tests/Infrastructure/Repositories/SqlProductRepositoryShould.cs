using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Infrastructure.Core.Database;

namespace Papyrus.Tests.Infrastructure.Repositories {
    [TestFixture]
    public class SqlProductRepositoryShould : SqlTest {
        private string anyProductId = "AnyProductID";
        private string anyProductName = "AnyProductName";
        private SqlProductRepository sqlProductRepository;

        [SetUp]
        public async void TruncateDataBase() {
            await dbConnection.Execute("TRUNCATE TABLE Product");
            await dbConnection.Execute("TRUNCATE TABLE ProductVersion");
            sqlProductRepository = new SqlProductRepository(dbConnection);
        }

        [Test]
        public async void load_a_product() {
            const string anyVersionId = "AnyVersionId";
            const string anyVersionName = "AnyVersionName";
            var versions = new List<ProductVersion> { new ProductVersion(anyVersionId, anyVersionName, DateTime.Today) };
            await InsertProduct(new Product(anyProductId, anyProductName, versions));

            var product = await sqlProductRepository.GetProduct(anyProductId);

            product.Id.Should().Be(anyProductId);
            product.Name.Should().Be(anyProductName);
            product.Versions.Count.Should().Be(1);
            var productVersion = product.Versions.First();
            productVersion.VersionId.Should().Be(anyVersionId);
            productVersion.VersionName.Should().Be(anyVersionName);
        }

        [Test]
        public async void load_a_product_with_its_versions() {
            var versions = new List<ProductVersion>
            {
                new ProductVersion("versionId1", "version1", DateTime.Today.AddDays(-1)),
                new ProductVersion("versionId2", "version2", DateTime.Today)
            };
            await InsertProduct(new Product(anyProductId, anyProductName, versions));

            var product = await sqlProductRepository.GetProduct(anyProductId);

            product.Id.Should().Be(anyProductId);
            product.Versions.Should().Contain((version) => version.VersionId.Equals("versionId1"));
            product.Versions.Should().Contain((version) => version.VersionId.Equals("versionId2"));
            product.Versions.Count.Should().Be(2);
        }

        [Test]
        public async void return_null_when_try_to_load_an_no_existing_product() {
            var product = await sqlProductRepository.GetProduct("AnyId");

            product.Should().Be(null);
        }

        [Test]
        public async Task load_a_list_with_all_products_containing_its_versions() {
            var versionsForPapyrus = new List<ProductVersion>
            {
                new ProductVersion("AnyIdForPapyrus", "AnyVersion", DateTime.Today.AddDays(-1)),
                new ProductVersion("AnotherIdForPapyrus", "AnyVersion", DateTime.Today)
            };
            var versionsForOpportunity = new List<ProductVersion>
            {
                new ProductVersion("AnyIdForOpportunity", "AnyVersion", DateTime.Today)
            };
            var papyrusId = "PapyrusId";
            await InsertProduct(new Product(papyrusId, "Papyrus", versionsForPapyrus));
            var opportunityId = "OpportunityId";
            await InsertProduct(new Product(opportunityId, "Opportunity", versionsForOpportunity));

            var products = await sqlProductRepository.GetAllDisplayableProducts();

            products.Should().Contain(prod => prod.ProductId == "PapyrusId");
            products.Should().Contain(prod => prod.ProductId == "OpportunityId");
            products.ToArray().Length.Should().Be(2);
        }

        [Test]
        public async Task gets_range_that_goes_from_first_version_to_the_latest_one() {
            var versions = new List<ProductVersion>
            {
                new ProductVersion("FirstVersionId", "1.0", DateTime.Today.AddDays(-3)),
                new ProductVersion("SecondVersionId", "2.0", DateTime.Today.AddDays(-2)),
                new ProductVersion("ThirdVersionId", "3.0", DateTime.Today.AddDays(-1)),
            };
            var product = new Product("PapyrusId", "Papyrus", versions);
            await InsertProduct(product);

            var fullRange = await sqlProductRepository.GetFullVersionRangeForProduct(product.Id);

            fullRange.FirstVersionId.Should().Be("FirstVersionId");
            fullRange.LatestVersionId.Should().Be("ThirdVersionId");
        }

        [Test]
        public async Task gets_all_versions_for_a_given_product() {
            var papyrusVersions = new List<ProductVersion>
            {
                new ProductVersion("FirstVersionId", "1.0", DateTime.Today.AddDays(-3)),
                new ProductVersion("SecondVersionId", "2.0", DateTime.Today.AddDays(-2)),
            };
            var product = new Product("PapyrusId", "Papyrus", papyrusVersions);
            await InsertProduct(product);

            var versions = await sqlProductRepository.GetAllVersionsFor("PapyrusId");

            versions.ShouldAllBeEquivalentTo(papyrusVersions);
        }

        [Test]
        public async Task get_last_version_for_a_given_product() {
            var thirdVersion = new ProductVersion("ThirdVersionId", "3.0", DateTime.Today.AddDays(-1));
            var papyrusVersions = new List<ProductVersion>
            {
                new ProductVersion("FirstVersionId", "1.0", DateTime.Today.AddDays(-3)),
                new ProductVersion("SecondVersionId", "2.0", DateTime.Today.AddDays(-2)),
                thirdVersion,
            };
            foreach (var papyrusVersion in papyrusVersions) {
                await InsertProductVersion(papyrusVersion, "PapyrusId");
            }

            var lastVersion = await sqlProductRepository.GetLastVersionForProduct("PapyrusId");

            lastVersion.ShouldBeEquivalentTo(thirdVersion);
        }

        [Test]
        public async Task get_product_with_wished_versions() {
            var version1 = new ProductVersion("AnyID", "1", DateTime.Today.AddDays(-20));
            var version2 = new ProductVersion("AnyOtherID", "2", DateTime.Today);
            var product = new Product("OpportunityID", "Opportunity", new List<ProductVersion>{ version1, version2});
            await InsertProduct(product);

            var filteredProduct = await sqlProductRepository.GetProductForVersions(product, new List<string> {"1"});

            filteredProduct.Versions.Should().HaveCount(1);
            filteredProduct.Versions.Should().Contain(v => v.VersionName == "1");
        }

        private async Task InsertProduct(Product product) {
            await dbConnection.Execute(@"INSERT INTO Product(ProductId, ProductName) 
                                VALUES (@ProductId, @ProductName);",
                                new {
                                    ProductId = product.Id,
                                    ProductName = product.Name,
                                });
            await InsertProductVersions(product);
        }

        private async Task InsertProductVersions(Product product) {
            foreach (var productVersion in product.Versions) {
                await InsertProductVersion(productVersion, product.Id);
            }
        }

        private async Task InsertProductVersion(ProductVersion productVersion, string productId) {
            await dbConnection.Execute(@"INSERT INTO ProductVersion(VersionId, VersionName, ProductId, Release) 
                                VALUES (@VersionId, @VersionName, @ProductId, @Release);",
                new {
                    VersionId = productVersion.VersionId,
                    VersionName = productVersion.VersionName,
                    ProductId = productId,
                    Release = productVersion.Release
                });
        }
    }
}