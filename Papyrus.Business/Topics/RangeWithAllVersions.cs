using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public class RangeWithAllVersions
    {
        public VersionRange VersionRange { get; private set; }
        public List<ProductVersion> Versions { get; private set; } 

        public RangeWithAllVersions(List<ProductVersion> versions, VersionRange versionRange)
        {
            VersionRange = versionRange;
            Versions = versions;
        }

        public bool Intersect(RangeWithAllVersions rangeWithAllVersions)
        {
            return this.Versions.Intersect(rangeWithAllVersions.Versions).Any();
        }

        public List<Collision> CollissionsWith(IEnumerable<RangeWithAllVersions> otherRanges)
        {
            var collisions = new List<Collision>();
            foreach (var rangeWithAllVersions in otherRanges)
            {
                if (this.Intersect(rangeWithAllVersions))
                {
                    collisions.Add(new Collision(this.ToEditableVersionRange(),
                        rangeWithAllVersions.ToEditableVersionRange()));
                }
            }
            return collisions;
        }

        private EditableVersionRange ToEditableVersionRange()
        {
            return new EditableVersionRange
            {
                FromVersion = Versions.First(vr => vr.VersionId == this.VersionRange.FromVersionId),
                ToVersion = Versions.First(vr => vr.VersionId == this.VersionRange.ToVersionId)
            };
        }
    }
}