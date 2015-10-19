using System;
using System.Dynamic;

namespace Papyrus.Business.Topics
{
    public class Document
    {
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }
        public string DocumentId { get; private set; }
        public string Language { get; }

        public Document(string title, string description, string content, string language)
        {
            Title = title;
            Description = description;
            Content = content;
            Language = language;
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