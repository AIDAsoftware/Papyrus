using System.Collections.ObjectModel;
using System.Linq;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Desktop
{
    public class EditableVersionRange
    {
        public ObservableCollection<EditableDocument> Documents { get; set; }
        public ProductVersion FromVersion { get; set; }
        public ProductVersion ToVersion { get; set; }

        public EditableVersionRange()
        {
            Documents = new ObservableCollection<EditableDocument>();
        }

        public VersionRange ToVersionRange()
        {
            var versionRange = new VersionRange(FromVersion, ToVersion);
            foreach (var editableDocument in Documents)
            {
                versionRange.Documents.Add(editableDocument.ToDocument());
            }
            return versionRange;
        }

        protected bool Equals(EditableVersionRange other) {
            return Documents.SequenceEqual(other.Documents) && Equals(FromVersion, other.FromVersion) && Equals(ToVersion, other.ToVersion);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EditableVersionRange) obj);
        }

        public EditableVersionRange Clone() {
            return new EditableVersionRange {
                Documents = new ObservableCollection<EditableDocument>(
                                Documents.Select(x => x.Clone())),
                FromVersion = FromVersion.Clone(),
                ToVersion = ToVersion.Clone()
            };
        }
    }
}