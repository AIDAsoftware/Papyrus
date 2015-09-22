using System.Collections.Generic;

namespace Papyrus.Business.Products
{
    using System;

    public class Product
    {
        public string Id { get; private set; }
        public string Description { get; private set; }

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

        public Product(string id, List<ProductVersion> productVersions, string description = "") {
            Id = id;
            Versions = productVersions;
            Description = description;
        }

        public Product WithDescription(string description)
        {
            Description = description;
            return this;
        }

        private void GenerateAutomaticId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class ProductVersion {
        public string VersionId   { get; private set; }
        public string VersionName { get; private set; }
        public string ProductName { get; private set; }

        public ProductVersion(string versionId, string versionName, string productName) {
            VersionId = versionId;
            VersionName = versionName;
            ProductName = productName;
        }
    }
}