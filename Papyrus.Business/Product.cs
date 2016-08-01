using System.Collections.Generic;

namespace Papyrus.Business {
    public class Product {
        public string Id { get; }
        public string Name { get; }
        public List<ProductVersion> Versions { get; }

        public Product(string id, string name, List<ProductVersion> productVersions) {
            Id = id;
            Name = name;
            Versions = productVersions;
        }
    }
}