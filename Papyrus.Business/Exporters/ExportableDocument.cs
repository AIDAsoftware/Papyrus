using System.IO;

namespace Papyrus.Business.Exporters {
    public class ExportableDocument {
        private readonly MkdocsFileNameConverter mkdocsFileNameConverter;
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Route { get; private set; }

        public ExportableDocument(string title, string content, string route) {
            Title = title;
            Content = content;
            Route = route;
        }
    }
}