using System;

namespace Papyrus.Business.Documents
{
    public class Document
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public string DocumentId { get; private set; }
        public string Language { get; private set; }
        public int Order { get; }

        public Document(string title, string description, string content, string language) :
            this(title, description, content, language, int.MaxValue){}

        public Document(string title, string description, string content, string language, int order) {
            if (string.IsNullOrEmpty(title)) throw new CannotCreateDocumentsWithoutTitleException();
            Title = title;
            Description = description;
            Content = content;
            Language = language;
            Order = order;
        }


        public Document WithId(string id)
        {
            DocumentId = id;
            return this;
        }

        public void GenerateAutomaticId()
        {
            DocumentId = Guid.NewGuid().ToString();
        }
    }
}