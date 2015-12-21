using System.IO;

namespace Papyrus.Business.Exporters {
    public class ExportableDocument {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Route { get; private set; }

        public string ExportableTitle {
            get {
                var invalidChars = Path.GetInvalidFileNameChars();
                var exportableTitle = Title;
                foreach (var invalidChar in invalidChars) {
                    exportableTitle = exportableTitle.Replace(invalidChar.ToString(), "-");
                }
                return exportableTitle;
            }
        }

        public ExportableDocument(string title, string content, string route) {
            Title = title;
            Content = content;
            Route = route;
        }
    }
}