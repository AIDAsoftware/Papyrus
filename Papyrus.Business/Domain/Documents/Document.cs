using Papyrus.Business.Domain.Products;

namespace Papyrus.Business.Domain.Documents {
    public class Document {
        public DocumentId Id { get; }
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public string Language { get; }
        public VersionIdentifier VersionIdentifier { get; }

        public Document(DocumentId id, string title, string description, string content, string language, VersionIdentifier versionId) {
            Id = id;
            Title = title;
            Description = description;
            Content = content;
            Language = language;
            VersionIdentifier = versionId;
        }
    }

    public class DocumentId {
        public string Value { get; }

        public DocumentId(string value) {
            Value = value;
        }
    }
}