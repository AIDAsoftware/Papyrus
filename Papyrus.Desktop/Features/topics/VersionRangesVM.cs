using System.Collections.ObjectModel;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangesVM
    {
        public ObservableCollection<EditableVersionRange> VersionRanges { get; private set; }

        public VersionRangesVM()
        {
            VersionRanges = new ObservableCollection<EditableVersionRange>();
        }

        public VersionRangesVM(ObservableCollection<EditableVersionRange> versionRanges)
        {
            VersionRanges = versionRanges;
        }

        public void Initialize()
        {
            
        }
    }
}