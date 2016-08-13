namespace Papyrus.Business.Domain.Documents {
    public class DocumentDto {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public string VersionId { get; set; }
        public string ProductId { get; set; }
    }
}