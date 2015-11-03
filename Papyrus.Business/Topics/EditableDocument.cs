using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Papyrus.Business.Topics
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

        public void ToDocument(VersionRange versionRange)
        {
            versionRange.AddDocument(new Document(
                title: Title,
                description: Description,
                content: Content,
                language: Language
                ));
        }
    }
}