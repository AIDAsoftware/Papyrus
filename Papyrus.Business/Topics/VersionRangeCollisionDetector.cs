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
            var collisions = new List<Collision>();
            var analylized = new List<RangeWithAllVersions>();
            foreach (var currentRange in rangesWithAllVersions)
            {
                analylized.Add(currentRange);
                foreach (var rangeWithAllVersions in rangesWithAllVersions)
                {
                    if (!analylized.Contains(rangeWithAllVersions) && currentRange.Versions.Intersect(rangeWithAllVersions.Versions).Any())
                    {
                        collisions.Add(new Collision(ToEditableVersionRange(currentRange.VersionRange),
                            ToEditableVersionRange(rangeWithAllVersions.VersionRange)));
                    }
                }
            }
            return collisions;
        }

        private List<RangeWithAllVersions> MapToRangeWithAllVersions(List<VersionRange> versionRanges)
        {
            var rangesWithAllVersions = new List<RangeWithAllVersions>();
            foreach (var versionRange in versionRanges)
            {
                var versions = AllVersionsContainedIn(versionRange);
                var rangeWithAllVersions = new RangeWithAllVersions(versions, versionRange);
                rangesWithAllVersions.Add(rangeWithAllVersions);
            }
            return rangesWithAllVersions;
        }

        private List<ProductVersion> AllVersionsContainedIn(VersionRange versionRange)
        {
            return Versions.Where(v => ReleaseFor(versionRange.FromVersionId) <= v.Release &&
                                       v.Release <= ReleaseFor(versionRange.ToVersionId)).ToList();
        }

        private EditableVersionRange ToEditableVersionRange(VersionRange versionRange)
        {
            return new EditableVersionRange
            {
                FromVersion = Versions.First(vr => vr.VersionId == versionRange.FromVersionId),
                ToVersion = Versions.First(vr => vr.VersionId == versionRange.ToVersionId)
            };
        }

        private DateTime ReleaseFor(string versionId)
        {
            return Versions.First(vr => versionId == vr.VersionId).Release;
        }
    }
}