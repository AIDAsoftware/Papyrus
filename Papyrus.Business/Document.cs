namespace Papyrus.Business {
    public class Document {
        public static Document CreateDocument(string title, string description, string content, string language, string productId, string versionId) {
            return new Document(title, description, content, language, new VersionIdentifier(productId, versionId));
        }

        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public string Language { get; }
        public VersionIdentifier VersionIdentifier { get; }

        private Document(string title, string description, string content, string language, VersionIdentifier versionId) {
            Title = title;
            Description = description;
            Content = content;
            Language = language;
            VersionIdentifier = versionId;
        }
    }

    public class VersionIdentifier {
        public string ProductId { get; }
        public string VersionId { get; }

        public VersionIdentifier(string productId, string versionId) {
            ProductId = productId;
            VersionId = versionId;
        }
    }
}