using System.Collections.Generic;

namespace Papyrus.Business.Topics
{
    public class EditableVersionRange
    {
        public string FromVersion { get; set; }
        public string ToVersion { get; set; }
        public Dictionary<string, EditableDocument> Documents { get; set; }

        public EditableVersionRange(string fromVersion, string toVersion, Dictionary<string, EditableDocument> documents)
        {
            FromVersion = fromVersion;
            ToVersion = toVersion;
            Documents = documents;
        }
    }
}