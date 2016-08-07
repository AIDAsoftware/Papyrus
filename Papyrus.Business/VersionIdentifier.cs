namespace Papyrus.Business {
    public class VersionIdentifier {
        public string ProductId { get; }
        public string VersionId { get; }

        public VersionIdentifier(string productId, string versionId) {
            ProductId = productId;
            VersionId = versionId;
        }
    }
}