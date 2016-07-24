namespace Papyrus.Tests {
    public class TestProductVersion {
        public string ProductId { get ; }
        public string VersionId { get; }

        public TestProductVersion(string productId, string versionId) {
            ProductId = productId;
            VersionId = versionId;
        }
    }
}