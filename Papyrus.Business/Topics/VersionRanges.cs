using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class VersionRanges : IEnumerable<VersionRange>
    {
        IList<VersionRange> versions = new List<VersionRange>();  

        public void Add(VersionRange versionRange)
        {
            versions.Add(versionRange);
        }

        public bool HasAnyRange()
        {
            return versions.Any();
        }

        public VersionRange this[int index]
        {
            get { return versions[index]; }
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