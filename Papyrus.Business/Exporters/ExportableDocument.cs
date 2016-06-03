namespace Papyrus.Business.Exporters {
    public class ExportableDocument {
        public string Title { get; private set; }
        public string Content { get; private set; }

        public ExportableDocument(string title, string content) {
            Title = title;
            Content = content;
        }
    }
}