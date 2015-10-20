using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangesVM
    {
        public ObservableCollection<EditableVersionRange> VersionRanges { get; }

        public VersionRangesVM()
        {
            VersionRanges = new ObservableCollection<EditableVersionRange>
            {
                new EditableVersionRange {FromVersionId = "1", ToVersionId = "2"},
                new EditableVersionRange {FromVersionId = "3", ToVersionId = "3"},
                new EditableVersionRange {FromVersionId = "4", ToVersionId = "7"}
            };
        }

        public void Initialize()
        {
            
        }
    }
}