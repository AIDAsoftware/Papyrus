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

            this.Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
