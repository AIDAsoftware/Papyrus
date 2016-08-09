namespace Papyrus.Business.Domain.Products {
    public class ProductVersion {
        public string Id { get; }
        public string Name { get; }

        public ProductVersion(string id, string name) {
            Id = id;
            Name = name;
        }
    }
}