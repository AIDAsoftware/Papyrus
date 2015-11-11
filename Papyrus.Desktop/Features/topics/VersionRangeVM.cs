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

        public VersionRangeVM(EditableVersionRange editableVersionRange) : this()
        {
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

    public class DesignModeVersionRangeVM    {
        public DesignModeVersionRangeVM() {
            VersionRange = new EditableVersionRange();
            var editableDocument = new EditableDocument() {Language ="es-ES"};
            VersionRange.Documents.Add(editableDocument);
            SelectedDocument = editableDocument;
        }

        public EditableVersionRange VersionRange { get; set; }

        public EditableDocument SelectedDocument { get; set; }
    }
}