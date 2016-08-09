using System.Collections.Generic;
using Papyrus.Business;
using Papyrus.Business.Domain.Products;

namespace Papyrus.Infrastructure.Core {
    public class FileProduct : SerializableItem {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ProductVersion> ProductVersions { get; set; }
    }
}