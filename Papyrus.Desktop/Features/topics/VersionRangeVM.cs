using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Annotations;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeVM : INotifyPropertyChanged
    {
        public ObservableCollection<ProductVersion> ProductVersions { get; private set; }

        private EditableDocument selectedDocument;
        public EditableDocument SelectedDocument
        {
            get
            {
                return selectedDocument;
            }
            set
            {
                selectedDocument = value;
                OnPropertyChanged("SelectedDocument");
            }
        }

        private EditableVersionRange versionRange;
        public EditableVersionRange VersionRange
        {
            get
            {
                return versionRange;
            }
            set
            {
                versionRange = value;
                OnPropertyChanged("VersionRange");
                SelectDefaultDocument();
            }
        }

        public VersionRangeVM()
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
        }

        public VersionRangeVM(EditableVersionRange editableVersionRange)
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
            VersionRange = editableVersionRange;
        }

        private void SelectDefaultDocument()
        {
            SelectedDocument = VersionRange.Documents.First(d => d.Language == "es-ES");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}