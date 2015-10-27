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
    }
}