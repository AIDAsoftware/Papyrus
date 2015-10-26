using System.Collections.ObjectModel;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeVM
    {
        public EditableVersionRange VersionRange { get; set; }
        public ObservableCollection<ProductVersion> ProductVersions { get; private set; }

        public VersionRangeVM()
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
        }

        public VersionRangeVM(EditableVersionRange editableVersionRange)
        {
            ProductVersions = new ObservableCollection<ProductVersion>();
            VersionRange = editableVersionRange;
        }
    }
}