using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Papyrus.Business.Exporters
{
    public class ExportableTopic
    {
        public List<ExportableVersionRange> VersionRanges { get; private set; }
        public ExportableProduct Product { get; set; }

        public ExportableTopic(ExportableProduct product)
        {
            VersionRanges = new List<ExportableVersionRange>();
            Product = product;
        }

        public void AddVersionRange(ExportableVersionRange versionRange)
        {
            VersionRanges.Add(versionRange);
        }

        public async Task ExportTopicIn(DirectoryInfo directory, string extension)
        {
            foreach (var versionRange in VersionRanges)
            {
                await versionRange.ExportVersionRangeIn(directory, Product, extension);
            }
        }

        public async Task ExportForAllProductsStructureIn(DirectoryInfo languageDirectory, ExportableProduct product) {
            foreach (var exportableVersionRange in VersionRanges) {
                await exportableVersionRange.ExportForAllProductsStructure(languageDirectory, product);
            }
        }
    }
}