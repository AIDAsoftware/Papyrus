using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop
{
    public class MainWindowVM
    {
        public DisplayableProduct SelectedProduct { get; set; }
    }

    public class DesignModeMainWindowVM : MainWindowVM
    {
    }
}