using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public partial class VersionRangesUserControl : UserControl
    {
        public VersionRangesVM ViewModel
        {
            get { return (VersionRangesVM)DataContext; }
        }

        public VersionRangesUserControl()
        {
            InitializeComponent();
        }
    }
}
