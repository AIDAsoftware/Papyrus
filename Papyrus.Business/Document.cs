namespace Papyrus.Business {
    public class Document {
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public string Language { get; }
        public string ProductId { get; }
        public string VersionId { get; }

        public Document(string title, string description, string content, string language, string productId, string versionId) {
            Title = title;
            Description = description;
            Content = content;
            Language = language;
            ProductId = productId;
            VersionId = versionId;
        }
    }
}