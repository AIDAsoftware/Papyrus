using System;
using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Topics
{
    public class VersionRanges
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

        public void ForEach(Action<VersionRange> func)
        {
            foreach (var versionRange in versions)
            {
                func.Invoke(versionRange);   
            }
        }
    }
}