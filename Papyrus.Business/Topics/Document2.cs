using System.Dynamic;

namespace Papyrus.Business.Topics
{
    public class Document2
    {
        public string Title { get; }
        public string Description { get; }
        public string Content { get; }

        public Document2(string title, string description, string content)
        {
            Title = title;
            Description = description;
            Content = content;
        }
    }
}