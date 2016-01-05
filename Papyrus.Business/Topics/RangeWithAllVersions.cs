using System;
using System.Collections.Generic;
using System.Linq;
using Papyrus.Business.Products;

namespace Papyrus.Business.Topics
{
    public class RangeWithAllVersions
    {
        private VersionRange VersionRange { get; set; }
        private List<ProductVersion> Versions { get; set; } 

        public RangeWithAllVersions(List<ProductVersion> versions, VersionRange versionRange)
        {
            VersionRange = versionRange;
            Versions = versions;
        }

        public bool Intersect(RangeWithAllVersions rangeWithAllVersions)
        {
            return Versions.Intersect(rangeWithAllVersions.Versions).Any();
        }

        public List<Collision> CollissionsWith(IEnumerable<RangeWithAllVersions> otherRanges)
        {
            var collisions = new List<Collision>();
            foreach (var rangeWithAllVersions in otherRanges)
            {
                if (Intersect(rangeWithAllVersions))
                {
                    collisions.Add(new Collision(ToEditableVersionRange(), rangeWithAllVersions.ToEditableVersionRange()));
                }
            }
            return collisions;
        }

        private EditableVersionRange ToEditableVersionRange()
        {
            return new EditableVersionRange
            {
                FromVersion = VersionRange.FromVersion,
                ToVersion = VersionRange.ToVersion
            };
        }
    }
}