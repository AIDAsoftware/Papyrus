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
            version1,
            version2,
            version3,
            version4,
            version5,
        };
        private const string ProductId = "PapyrusId";

        private VersionRangeCollisionDetector versionRangeCollisionDetector;
        private ProductRepository productRepository;
        private static ProductVersion version1 = new ProductVersion("version1", "1.0", DateTime.Today.AddDays(-10));
        private static ProductVersion version2 = new ProductVersion("version2", "2.0", DateTime.Today.AddDays(-8));
        private static ProductVersion version3 = new ProductVersion("version3", "3.0", DateTime.Today.AddDays(-6));
        private static ProductVersion version4 = new ProductVersion("version4", "4.0", DateTime.Today.AddDays(-4));
        private static ProductVersion version5 = new ProductVersion("version5", "5.0", DateTime.Today.AddDays(-2));

        [SetUp]
        public void SetUp()
        {
            productRepository = Substitute.For<ProductRepository>();
            versionRangeCollisionDetector = new VersionRangeCollisionDetector(productRepository);
            productRepository.GetAllVersionsFor(ProductId).Returns(Task.FromResult(productVersionsFrom1To5));
        }

        [Test]
        public async Task detect_collision_when_toversion_of_a_version_range_is_equal_to_fromversion_in_any_other_range()
        {
            var topic = new Topic(ProductId);
            topic.AddVersionRange(new VersionRange("version1", "version2"));
            topic.AddVersionRange(new VersionRange("version3", "version4"));
            topic.AddVersionRange(new VersionRange("version4", "version5"));

            var collisions = await versionRangeCollisionDetector.CollisionsFor(topic);

            var expectedCollisions = new List<Collision>
            {
                new Collision(
                    new EditableVersionRange { FromVersion = version3, ToVersion = version4 },
                    new EditableVersionRange { FromVersion = version4, ToVersion = version5 })
            };
            collisions.ShouldAllBeEquivalentTo(expectedCollisions);
        }
        
        [Test]
        public async Task detect_collision_when_fromversion_of_a_version_range_is_contained_by_other_range()
        {
            var topic = new Topic(ProductId);
            topic.AddVersionRange(new VersionRange("version1", "version3"));
            topic.AddVersionRange(new VersionRange("version2", "version4"));

            var collisions = await versionRangeCollisionDetector.CollisionsFor(topic);

            var expectedCollisions = new List<Collision>
            {
                new Collision(
                    new EditableVersionRange { FromVersion = version1, ToVersion = version3 },
                    new EditableVersionRange { FromVersion = version2, ToVersion = version4 }
                )};
            collisions.ShouldAllBeEquivalentTo(expectedCollisions);
        }


        [Test]
        public async Task not_detect_collision_no_ranges_use_same_product_version()
        {
            var topic = new Topic(ProductId);
            topic.AddVersionRange(new VersionRange("version1", "version2"));
            topic.AddVersionRange(new VersionRange("version3", "version4"));
            topic.AddVersionRange(new VersionRange("version5", "version5"));

            var collisions = await versionRangeCollisionDetector.CollisionsFor(topic);

            collisions.Should().BeEmpty();
        }

    }
}