using System;

namespace Papyrus.Business.Exporters {
    public class ExportableDocument {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public int Order { get; }

        public ExportableDocument(string title, string content): this(title, content, int.MaxValue) {
        }

        public ExportableDocument(string title, string content, int order) {
            Title = title;
            Content = content;
            Order = order;
        }
    }
}