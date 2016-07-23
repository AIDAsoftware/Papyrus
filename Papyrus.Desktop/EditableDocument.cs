using System.ComponentModel;
using System.Runtime.CompilerServices;
using Papyrus.Business.Documents;

namespace Papyrus.Desktop
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
                OnPropertyChanged();
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }

        private string order = "200";
        public string Order {
            get { return order; }
            set {
                order = value;
                OnPropertyChanged();
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
                language: Language,
                order: string.IsNullOrEmpty(Order) ? int.MaxValue : int.Parse(Order)
            );
        }

        protected bool Equals(EditableDocument other) {
            return string.Equals(title, other.title) && 
                string.Equals(description, other.description) && 
                string.Equals(content, other.content) && 
                string.Equals(order, other.order) && 
                string.Equals(Language, other.Language);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EditableDocument) obj);
        }

        public EditableDocument Clone() {
            return new EditableDocument {
                Title = Title,
                Content = Content,
                Language = Language,
                Description = Description,
                Order = Order
            };
        }
    }
}