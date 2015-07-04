namespace Papyrus.Business.Documents
{
    using System;
    using Exceptions;

    public class Document
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public string Language { get; private set; }


        public Document WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public Document WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public Document WithContent(string content)
        {
            Content = content;
            return this;
        }

        public Document ForLanguage(string language)
        {
            Language = language;
            return this;
        }

        public Document WithId(string id)
        {
            if (!string.IsNullOrWhiteSpace(Id))
                throw new CannotModifyDocumentIdException();
            Id = id;
            return this;
        }

        public void GenerateAutomaticId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}