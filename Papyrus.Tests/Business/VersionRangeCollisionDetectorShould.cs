using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Tests.Business
{
    [TestFixture]
    public class VersionRangeCollisionDetectorShould
    {
        private List<ProductVersion> productVersionsFrom1To5 = new List<ProductVersion>
        {
            new ProductVersion("version1", "1.0", DateTime.Today.AddDays(-10)),
            new ProductVersion("version2", "2.0", DateTime.Today.AddDays(-8)),
            new ProductVersion("version3", "3.0", DateTime.Today.AddDays(-6)),
            new ProductVersion("version4", "4.0", DateTime.Today.AddDays(-4)),
            new ProductVersion("version5", "5.0", DateTime.Today.AddDays(-2)),
        };

        [Test]
        public async Task detect_collision_when_toversion_of_a_version_range_is_equal_to_fromversion_in_any_other_range()
        {
            var productRepository = Substitute.For<ProductRepository>();
            var productId = "PapyrusId";
            productRepository.GetAllVersionsFor(productId).Returns(Task.FromResult(productVersionsFrom1To5));
            var topic = new Topic(productId);
            topic.AddVersionRange(new VersionRange("version1", "version2"));
            topic.AddVersionRange(new VersionRange("version3", "version4"));
            topic.AddVersionRange(new VersionRange("version4", "version5"));

            var versionRangeCollisionDetector = new VersionRangeCollisionDetector(productRepository);
            var isThereCollision = await versionRangeCollisionDetector.IsThereAnyCollisionFor(topic);

            isThereCollision.Should().BeTrue();
        }
        
        [Test]
        public async Task detect_collision_when_fromversion_of_a_version_range_is_contained_by_other_range()
        {
            var productRepository = Substitute.For<ProductRepository>();
            const string productId = "PapyrusId";
            productRepository.GetAllVersionsFor(productId).Returns(Task.FromResult(productVersionsFrom1To5));
            var topic = new Topic(productId);
            topic.AddVersionRange(new VersionRange("version1", "version3"));
            topic.AddVersionRange(new VersionRange("version2", "version4"));

            var versionRangeCollisionDetector = new VersionRangeCollisionDetector(productRepository);
            var isThereCollision = await versionRangeCollisionDetector.IsThereAnyCollisionFor(topic);

            isThereCollision.Should().BeTrue();
        }
    }
}