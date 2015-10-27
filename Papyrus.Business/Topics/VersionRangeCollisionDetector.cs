using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public class VersionRangeCollisionDetector
    {
        private readonly ProductRepository productRepository;
        private List<ProductVersion> Versions { get; set; }


        public VersionRangeCollisionDetector(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public virtual async Task<List<Collision>> CollisionsFor(Topic topic)
        {
            Versions = await productRepository.GetAllVersionsFor(topic.ProductId);
            var versionRanges = new List<VersionRange>(topic.VersionRanges);
            var rangesWithAllVersions = MapToRangeWithAllVersions(versionRanges);
            return CollisionsIn(rangesWithAllVersions);
        }

        private List<Collision> CollisionsIn(List<RangeWithAllVersions> rangesWithAllVersions)
        {
            var allCollisions = new List<Collision>();
            var analylized = new List<RangeWithAllVersions>();
            foreach (var currentRange in rangesWithAllVersions)
            {
                analylized.Add(currentRange);
                var collisions = currentRange.CollissionsWith(rangesWithAllVersions.Except(analylized));
                allCollisions.AddRange(collisions);
            }
            return allCollisions;
        }

        private List<RangeWithAllVersions> MapToRangeWithAllVersions(List<VersionRange> versionRanges)
        {
            return versionRanges.Select(ToRangeWithAllVersions).ToList();
        }

        private RangeWithAllVersions ToRangeWithAllVersions(VersionRange versionRange)
        {
            var versions = AllVersionsContainedIn(versionRange);
            var rangeWithAllVersions = new RangeWithAllVersions(versions, versionRange);
            return rangeWithAllVersions;
        }

        private List<ProductVersion> AllVersionsContainedIn(VersionRange versionRange)
        {
            return Versions.Where(v => ReleaseFor(versionRange.FromVersionId) <= v.Release &&
                                       v.Release <= ReleaseFor(versionRange.ToVersionId)).ToList();
        }

        private DateTime ReleaseFor(string versionId)
        {
            return Versions.First(vr => versionId == vr.VersionId).Release;
        }
    }
}