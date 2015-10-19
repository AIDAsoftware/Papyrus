using System;
using System.Dynamic;

namespace Papyrus.Business.Topics
{
    public class Document2
    {
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public string DocumentId { get; private set; }
        public string Language { get; }

        public Document2(string title, string description, string content)
        {
            Title = title;
            Description = description;
            Content = content;
        }

        public Document2(string title, string description, string content, string language)
            : this(title, description, content)
        {
            Language = language;
        }

        public Document2 WithId(string id)
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