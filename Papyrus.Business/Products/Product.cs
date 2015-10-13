using System.Collections.Generic;

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
    }

    public class ProductVersion {
        //TODO: rename properties
        public string VersionId   { get; private set; }
        public string VersionName { get; private set; }
        public DateTime Release { get; }

        public ProductVersion(string versionId, string versionName, DateTime release)
        {
            VersionId = versionId;
            VersionName = versionName;
            Release = release;
        }
    }
}