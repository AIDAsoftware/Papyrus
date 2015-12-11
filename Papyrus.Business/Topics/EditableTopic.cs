using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Papyrus.Business.Topics.Exceptions;

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
                OnPropertyChanged("Product");
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
                editableVersionRange.ToVersionRange(topic);
            }
            return topic;
        }
    }
}