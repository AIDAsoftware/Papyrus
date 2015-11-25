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
            return this.Versions.Intersect(rangeWithAllVersions.Versions).Any();
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
                FromVersion = GetVersionById(VersionRange.FromVersionId),
                ToVersion = GetVersionById(VersionRange.ToVersionId)
            };
        }

        private ProductVersion GetVersionById(string versionId)
        {
            if (versionId == "*") return new ProductVersion("*", "Last version", DateTime.MaxValue);
            return Versions.First(vr => vr.VersionId == versionId);
        }
    }
}