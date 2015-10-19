using System.Collections.Generic;

namespace Papyrus.Business.Topics
{
    public class EditableVersionRange
    {
        public string FromVersionId { get; set; }
        public string ToVersionId { get; set; }
        public List<EditableDocument> Documents { get; private set; }

        public EditableVersionRange()
        {
            Documents = new List<EditableDocument>();
        }
    }
}