using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Desktop.Util.Command;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangesVM
    {
        public ObservableCollection<EditableVersionRange> VersionRanges { get; protected set; }
        public EditableVersionRange SelectedVersionRange { get; set; }
        public IAsyncCommand DeleteVersionRange { get; private set; }
        public DisplayableProduct SelectedProduct { get; set; }

        public VersionRangesVM()
        {
            VersionRanges = new ObservableCollection<EditableVersionRange>();
            DeleteVersionRange = RelayAsyncSimpleCommand.Create(DeleteCurrentVersionRange, () => true);
        }

        private async Task DeleteCurrentVersionRange()
        {
            VersionRanges.Remove(SelectedVersionRange);
            Console.WriteLine("Remove");
        }

        public VersionRangesVM(EditableTopic editableTopic) : this()
        {
            VersionRanges = editableTopic.VersionRanges;
            SelectedProduct = editableTopic.Product;
        }

        public void Initialize()
        {
            
        }
    }

    public class DesignModeVersionRangesVM : VersionRangesVM
    {
        public DesignModeVersionRangesVM()
        {
            var visibleEditableVersionRange = new EditableVersionRange
            {
                FromVersion = new ProductVersion("AnyId", "1.0", DateTime.Today),
                ToVersion = new ProductVersion("AnyId", "2.0", DateTime.Today),
            };
            visibleEditableVersionRange.Documents.Add(new EditableDocument
            {
                Title = "Título",
                Description = "Descripción",
                Content = "Contenido",
                Language = "es-ES"
            });
            VersionRanges = new ObservableCollection<EditableVersionRange>
            {
                visibleEditableVersionRange,
                new EditableVersionRange
                {
                    FromVersion = new ProductVersion("AnyId", "3.0", DateTime.Today),
                    ToVersion = new ProductVersion("AnyId", "4.0", DateTime.Today)
                }
            };
        }
    }
}