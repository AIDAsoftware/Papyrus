using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

            DataContext = new VersionRangesVM();
            this.Loaded += VersionRangesUserControl_Loaded;
        }

        private void VersionRangesUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
