namespace Papyrus.Business.Products
{
    using System;

    public class Product
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Product() {
            GenerateAutomaticId();
        }

        public Product(string name): this() {
            this.Name = name;
        }

        public Product(string id, string name) {
            this.Id = id;
            this.Name = name;
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
}