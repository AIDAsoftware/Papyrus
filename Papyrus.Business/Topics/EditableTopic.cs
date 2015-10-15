using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}