namespace Papyrus.Business
{
    using System;
    using Tests;

    public class Document
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public string Language { get; private set; }


        public Document WithTitle(string title)
        {
            this.Title = title;
            return this;
        }

        public Document WithDescription(string description)
        {
            this.Description = description;
            return this;
        }

        public Document WithContent(string content)
        {
            this.Content = content;
            return this;
        }

        public Document ForLanguage(string language)
        {
            this.Language = language;
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