using Papyrus.Infrastructure.Core;

namespace Papyrus.Infrastructure.Repositories {
    public class SerializableDocument : SerializableItem {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ProductId { get; set; }
        public string VersionId { get; set; }
    }
}