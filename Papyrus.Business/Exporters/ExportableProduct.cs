using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Papyrus.Business.Exporters {
    public class ExportableProduct {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public IEnumerable<ExportableTopic> Topics { get; set; }

        public ExportableProduct(string productId, string productName) {
            ProductId = productId;
            ProductName = productName;
        }

        public DirectoryInfo ExportIn(DirectoryInfo directory) {
            return directory.CreateSubdirectory(ProductName);
        }

        public async Task ExportInAllProductsFormatIn(DirectoryInfo directory) {
            foreach (var topic in Topics) {
                await topic.ExportForAllProductsStructureIn(directory, this);
            }
        }
    }
}