namespace Papyrus.Business {
    public class VersionIdentifier {
        public string ProductId { get; }
        public string VersionId { get; }

        public VersionIdentifier(string productId, string versionId) {
            ProductId = productId;
            VersionId = versionId;
        }

        protected bool Equals(VersionIdentifier other) {
            return string.Equals(ProductId, other.ProductId) && string.Equals(VersionId, other.VersionId);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as VersionIdentifier;
            return other != null && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return ((ProductId != null ? ProductId.GetHashCode() : 0)*397) ^ (VersionId != null ? VersionId.GetHashCode() : 0);
            }
        }
    }
}