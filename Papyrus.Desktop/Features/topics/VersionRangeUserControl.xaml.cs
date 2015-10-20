using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Papyrus.Business.Topics;

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

            DataContext = new VersionRangeVM();
            this.Loaded += VersionRangeUserControl_Loaded;
        }

        private void VersionRangeUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }

    public class VersionRangeVM
    {
        public EditableVersionRange VersionRange { get; set; }

        public VersionRangeVM()
        {
            VersionRange = new EditableVersionRange
            {
                FromVersionId = "1",
                ToVersionId = "2",
            };
            VersionRange.Documents.AddRange(new List<EditableDocument>
            {
                new EditableDocument { Title = "Título", Description = "Descripción", Content = "Contenido", Language = "es-ES"},
                new EditableDocument { Title = "Title", Description = "Description", Content = "Content", Language = "en-GB"}
            });
        }

        public void Initialize()
        {
        }
    }
}
