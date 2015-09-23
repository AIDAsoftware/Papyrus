namespace Papyrus.Business.Documents
{
    using System;
    using Exceptions;

    public class Document
    {
        public DocumentIdentity DocumentIdentity { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }

        public Document()
        {
            DocumentIdentity = new DocumentIdentity();
        }

        public Document ForProductVersion(string versionId) {
            DocumentIdentity.VersionId = versionId;
            return this;
        }

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
            DocumentIdentity.Language = language;
            return this;
        }

        public Document WithTopicId(string id)
        {
            if (!string.IsNullOrWhiteSpace(DocumentIdentity.TopicId))
                throw new CannotModifyDocumentIdException();
            DocumentIdentity.TopicId = id;
            return this;
        }

        public Document ForProduct(string productId)
        {
            DocumentIdentity.ProductId = productId;
            return this;
        }

        public void GenerateAutomaticId()
        {
            DocumentIdentity.TopicId = Guid.NewGuid().ToString();
        }
    }

    public class DocumentIdentity
    {
        public string Language { get; set; }
        public string TopicId { get; set; }
        public string ProductId { get; set; }
        public string VersionId { get; set; }
    }
}