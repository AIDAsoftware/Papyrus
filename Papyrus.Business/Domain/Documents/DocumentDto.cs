namespace Papyrus.Business.Domain.Documents {
    // TODO: replace this class for Document (they are equal for now)
    public class DocumentDto {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public string VersionId { get; set; }
        public string ProductId { get; set; }
    }
}