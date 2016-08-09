using System.Collections.Generic;
using Papyrus.Business.Domain.Products;
using Papyrus.Infrastructure.Core;

namespace Papyrus.Infrastructure.Repositories {
    public class SerializableProduct : SerializableItem {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ProductVersion> ProductVersions { get; set; }
    }
}