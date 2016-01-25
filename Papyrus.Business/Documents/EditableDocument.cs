using System.ComponentModel;
using System.Runtime.CompilerServices;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Business.Documents
{
    public class EditableDocument : INotifyPropertyChanged
    {
        public string Language { get; set; }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private string content;

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Document ToDocument()
        {
            return new Document(
                title: Title,
                description: Description,
                content: Content,
                language: Language
                );
        }

        protected bool Equals(EditableDocument other) {
            return string.Equals(title, other.title) && string.Equals(description, other.description) && string.Equals(content, other.content) && string.Equals(Language, other.Language);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EditableDocument) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (title != null ? title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (description != null ? description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (content != null ? content.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Language != null ? Language.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}