namespace Papyrus.Tests
{
    using System;

    public class Document
    {
        private string title;
        private string description;
        private string content;
        private string language;


        public Document WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public Document WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public Document WithContent(string content)
        {
            this.content = content;
            return this;
        }

        public Document ForLanguage(string language)
        {
            this.language = language;
            return this;
        }
    }
}