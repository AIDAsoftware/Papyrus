using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Papyrus.Business.Exporters
{
    public class ExportableTopic
    {
        public List<ExportableVersionRange> VersionRanges { get; private set; }

        public ExportableTopic()
        {
            VersionRanges = new List<ExportableVersionRange>();
        }

        public void AddVersion(ExportableVersionRange versionRange)
        {
            VersionRanges.Add(versionRange);
        }

        public async Task ExportTopicIn(DirectoryInfo directory)
        {
            foreach (var versionRange in VersionRanges)
            {
                await versionRange.ExportVersionRangeIn(directory);
            }
        }
    }
}