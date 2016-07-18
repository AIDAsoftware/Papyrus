using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Papyrus.Business.Documents;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Business.Topics
{
    public class EditableTopic : INotifyPropertyChanged
    {
        public string TopicId { get; set; }
        public ObservableCollection<EditableVersionRange> VersionRanges { get; set; }

        private DisplayableProduct product;
        public DisplayableProduct Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditableTopic()
        {
            VersionRanges = new ObservableCollection<EditableVersionRange>();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Topic ToTopic()
        {
            var topic = new Topic(Product.ProductId).WithId(TopicId);
            foreach (var editableVersionRange in VersionRanges)
            {
                topic.AddVersionRange(editableVersionRange.ToVersionRange());
            }
            return topic;
        }

        protected bool Equals(EditableTopic other) {
            return Equals(product, other.product) && string.Equals(TopicId, other.TopicId) && VersionRanges.SequenceEqual(other.VersionRanges);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EditableTopic) obj);
        }

        public EditableTopic Clone() {
            var newTopic = new EditableTopic {
                Product = Product.Clone(),
                TopicId = TopicId,
            };
            var versionRanges = new ObservableCollection<EditableVersionRange>();
            foreach (var range in VersionRanges) {
                var copy = range.Clone();
                versionRanges.Add(copy);
            }
            newTopic.VersionRanges = versionRanges;
            return newTopic;
        }
    }
}