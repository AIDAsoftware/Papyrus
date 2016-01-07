using System.Collections.ObjectModel;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Business.VersionRanges
{
    public class EditableVersionRange
    {
        public ObservableCollection<EditableDocument> Documents { get; private set; }
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
    }
}