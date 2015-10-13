namespace Papyrus.Business.Topics
{
    public struct LanguageDocumentPair
    {
        public string Language { get; }
        public Document2 Document { get; }

        public LanguageDocumentPair(string language, Document2 document)
        {
            Language = language;
            Document = document;
        }
    }
}