using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Papyrus.Business.Documents;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Desktop.Annotations;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeVM : INotifyPropertyChanged
    {
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
                OnPropertyChanged();
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
                OnPropertyChanged();
                SelectDefaultDocument();
            }
        }

        public VersionRangeVM() { }
        public VersionRangeVM(EditableVersionRange editableVersionRange)
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