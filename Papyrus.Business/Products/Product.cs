using System.Collections.Generic;
using System.Linq;

namespace Papyrus.Business.Products
{
    using System;

    public class Product
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public List<ProductVersion> Versions { get; private set; }

        public Product() {
            GenerateAutomaticId();
            Versions = new List<ProductVersion>();
        }

        //TODO: check why we have 3 constructors
        public Product(string id)
        {
            Id = id;
            Versions = new List<ProductVersion>();
        }

        public Product(string id, string name, List<ProductVersion> productVersions) {
            Id = id;
            Name = name;
            Versions = productVersions;
        }

        private void GenerateAutomaticId()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public ProductVersion FirstVersion()
        {
            return Versions.OrderBy(productVersion => productVersion.Release).First();
        }
    }
}