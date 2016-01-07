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

        public void ToVersionRange(Topic topic)
        {
            var versionRange = new VersionRange(FromVersion, ToVersion);
            topic.AddVersionRange(versionRange);
            foreach (var editableDocument in Documents)
            {
                editableDocument.ToDocument(versionRange);
            }
        }
    }
}