using System.Collections.ObjectModel;

namespace Papyrus.Business.Topics
{
    public class EditableVersionRange
    {
        public string FromVersionId { get; set; }
        public string ToVersionId { get; set; }
        public ObservableCollection<EditableDocument> Documents { get; private set; }

        public EditableVersionRange()
        {
            Documents = new ObservableCollection<EditableDocument>();
        }

        public void ToVersionRange(Topic topic)
        {
            var versionRange = new VersionRange(FromVersionId, ToVersionId);
            topic.AddVersionRange(versionRange);
            foreach (var editableDocument in Documents)
            {
                editableDocument.ToDocument(versionRange);
            }
        }
    }
}