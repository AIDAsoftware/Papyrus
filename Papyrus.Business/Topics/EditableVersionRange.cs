using System.Collections.Generic;

namespace Papyrus.Business.Topics
{
    public class EditableVersionRange
    {
        public string FromVersionId { get; set; }
        public string ToVersionId { get; set; }
        public Dictionary<string, EditableDocument> Documents { get; set; }

        public EditableVersionRange(string fromVersionId, string toVersionId, Dictionary<string, EditableDocument> documents)
        {
            FromVersionId = fromVersionId;
            ToVersionId = toVersionId;
            Documents = documents;
        }
    }
}