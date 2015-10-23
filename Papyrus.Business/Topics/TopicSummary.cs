namespace Papyrus.Business.Topics
{
    public class TopicSummary
    {
        public string TopicId { get; set; }
        public DisplayableProduct Product { get; set; }
        public string VersionName { get; set; }
        public string LastDocumentTitle { get; set; }
        public string LastDocumentDescription { get; set; }
    }
}