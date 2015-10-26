using System.Windows.Controls;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class VersionRangeUserControl : UserControl
    {
        public VersionRangeVM ViewModel
        {
            get { return (VersionRangeVM) DataContext; }
        }

        public VersionRangeUserControl()
        {
            InitializeComponent();
        }
    }
}
