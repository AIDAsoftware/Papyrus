namespace Papyrus.Business.Exporters {
    public class ExportableProduct {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }

        public ExportableProduct(string productId, string productName) {
            ProductId = productId;
            ProductName = productName;
        }
    }
}