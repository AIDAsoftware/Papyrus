using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Papyrus.Business.Documents;
using Papyrus.Desktop.Features.Documents.Events;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop.Features.Documents {
    /// <summary>
    /// Interaction logic for NewDocumentWindow.xaml
    /// </summary>
    public partial class NewDocumentWindow : Window {
        public NewDocumentWindow() {
            InitializeComponent();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e) {
            var document = new Document().WithTitle("Test title");
            EventBus.Raise(new CreateNewDocument(document));
        }
    }
}
