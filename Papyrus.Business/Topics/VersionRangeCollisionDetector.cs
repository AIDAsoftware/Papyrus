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

        public virtual async Task<List<EditableVersionRange>> VersionRangesWithCollisionsFor(Topic topic)
        {
            Versions = await productRepository.GetAllVersionsFor(topic.ProductId);
            var versionRanges = topic.VersionRanges;
            var collidedVersionRanges = GetOnlyThoseWithCollisions(versionRanges);
            return MapToEditableVersionRange(collidedVersionRanges);
        }

        public virtual async Task<List<Collision>> CollisionsFor(Topic topic)
        {
            Versions = await productRepository.GetAllVersionsFor(topic.ProductId);
            var versionRanges = new List<VersionRange>(topic.VersionRanges);
            var rangesWithAllVersions = RangeWithAllVersionsFrom(versionRanges);
            var collisions = new List<Collision>();
            for (var j = 0; j < rangesWithAllVersions.Count - 1; j++)
            {
                for (var i = j + 1; i < rangesWithAllVersions.Count; i++)
                {
                    if (rangesWithAllVersions[j].Versions.Intersect(rangesWithAllVersions[i].Versions).Any())
                    {
                        collisions.Add(new Collision(ToEditableVersionRange(rangesWithAllVersions[j].VersionRange),
                            ToEditableVersionRange(rangesWithAllVersions[i].VersionRange)));
                    }
                }
            }
            return collisions;
        }

        private List<RangeWithAllVersions> RangeWithAllVersionsFrom(List<VersionRange> versionRanges)
        {
            var rangesWithAllVersions = new List<RangeWithAllVersions>();
            foreach (var versionRange in versionRanges)
            {
                var versions = Versions.Where(v => ReleaseFor(versionRange.FromVersionId) <= v.Release &&
                                                               v.Release <= ReleaseFor(versionRange.ToVersionId)).ToList();
                var rangeWithAllVersions = new RangeWithAllVersions(versions, versionRange);
                rangesWithAllVersions.Add(rangeWithAllVersions);
            }
            return rangesWithAllVersions;
        }

        private List<VersionRange> GetOnlyThoseWithCollisions(VersionRanges versionRanges)
        {
            return versionRanges.Where(versionRange => 
                DoesVersionRangeCollideWithAnyRangeIn(versionRange, versionRanges)).ToList();
        }

        private List<EditableVersionRange> MapToEditableVersionRange(IEnumerable<VersionRange> collidedVersionRanges)
        {
            var editableVersionRanges = new List<EditableVersionRange>();
            foreach (var versionRange in collidedVersionRanges)
            {
                var editableVersionRange = ToEditableVersionRange(versionRange);
                editableVersionRanges.Add(editableVersionRange);
            }
            return editableVersionRanges;
        }

        private EditableVersionRange ToEditableVersionRange(VersionRange versionRange)
        {
            return new EditableVersionRange
            {
                FromVersion = Versions.First(vr => vr.VersionId == versionRange.FromVersionId),
                ToVersion = Versions.First(vr => vr.VersionId == versionRange.ToVersionId)
            };
        }

        private bool DoesVersionRangeCollideWithAnyRangeIn(VersionRange versionRange, VersionRanges versionRanges)
        {
            var fromVersionId = versionRange.FromVersionId;
            return versionRanges.Where(vr => vr != versionRange)
                    .Any(vr => IsVersionIncludedInVersionRange(fromVersionId, vr));
        }

        private bool IsVersionIncludedInVersionRange(string fromVersionId, VersionRange versionRange)
        {
            return ReleaseFor(versionRange.FromVersionId) <= ReleaseFor(fromVersionId) &&
                   ReleaseFor(fromVersionId) <= ReleaseFor(versionRange.ToVersionId);
        }

        private DateTime ReleaseFor(string versionId)
        {
            return Versions.First(vr => versionId == vr.VersionId).Release;
        }
    }
}