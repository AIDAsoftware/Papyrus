namespace Papyrus.Business {
    public class Document {
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public string Language { get; }
        public VersionIdentifier VersionIdentifier { get; }

        public Document(string title, string description, string content, string language, VersionIdentifier versionId) {
            Title = title;
            Description = description;
            Content = content;
            Language = language;
            VersionIdentifier = versionId;
        }
    }
}