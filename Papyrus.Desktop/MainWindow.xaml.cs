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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Subscriber<ApplicationErrorOcurred> {
        public MainWindow() {
            InitializeComponent();
            EventBus.Subscribe(this);
        }

        public void Handle(ApplicationErrorOcurred domainEvent) {
            MessageBox.Show(domainEvent.Exception.Message);
        }
    }
}
