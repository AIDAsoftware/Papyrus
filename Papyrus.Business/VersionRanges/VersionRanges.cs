using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.VersionRanges
{
    public class VersionRanges : IEnumerable<VersionRange>
    {
        private readonly IList<VersionRange> versions = new List<VersionRange>();  

        public void Add(VersionRange versionRange)
        {
            versions.Add(versionRange);
        }

        public bool HasAnyRange()
        {
            return versions.Any();
        }

        public IEnumerator<VersionRange> GetEnumerator()
        {
            return versions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}