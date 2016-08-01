namespace Papyrus.Business {
    public interface SerializableItem {
        string Id { get; }
    }

    public class FileDocument : SerializableItem {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ProductId { get; set; }
        public string VersionId { get; set; }
    }
}