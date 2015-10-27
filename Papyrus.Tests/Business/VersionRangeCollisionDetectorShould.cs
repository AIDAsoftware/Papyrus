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
        // TODO
        //   - when to version of a version range is equal to fromVersion in any other range
        //   - when from version of a version range is contained  by other range
        // Examples:
        //   (1-2), (2-3)
        //   (1-3), (2-4)

        [Test]
        public async Task detect_collision_when_toversion_of_a_version_range_is_equal_to_fromversion_in_any_other_range()
        {
            var productRepository = Substitute.For<ProductRepository>();
            var productId = "PapyrusId";
            var topic = new Topic(productId);
            topic.AddVersionRange(new VersionRange("version1", "version2"));
            topic.AddVersionRange(new VersionRange("version3", "version4"));
            topic.AddVersionRange(new VersionRange("version4", "version5"));

            var versionRangeCollisionDetector = new VersionRangeCollisionDetector(productRepository);
            var isThereCollision = await versionRangeCollisionDetector.IsThereAnyCollisionFor(topic);

            isThereCollision.Should().BeTrue();
        }
    }
}