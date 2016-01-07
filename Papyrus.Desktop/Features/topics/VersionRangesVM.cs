using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Papyrus.Business.Documents;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;
using Papyrus.Desktop.Annotations;
using Papyrus.Desktop.Util.Command;
using Papyrus.Infrastructure.Core.DomainEvents;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangesVM : INotifyPropertyChanged
    {
        public ObservableCollection<EditableVersionRange> VersionRanges { get; protected set; }

        private EditableVersionRange selectedVersionRange;
        public EditableVersionRange SelectedVersionRange
        {
            get { return selectedVersionRange; }
            set
            {
                selectedVersionRange = value;
                OnPropertyChanged("SelectedVersionRange");
            }
        }

        public IAsyncCommand DeleteVersionRange { get; private set; }
        public IAsyncCommand CreateVersionRange { get; private set; }
        public DisplayableProduct SelectedProduct { get; set; }

        public VersionRangesVM()
        {
            VersionRanges = new ObservableCollection<EditableVersionRange>();
            DeleteVersionRange = RelayAsyncSimpleCommand.Create(DeleteCurrentVersionRange, () => true);
            CreateVersionRange = RelayAsyncSimpleCommand.Create(CreateNewVersionRange, () => true);
        }

        private async Task CreateNewVersionRange()
        {
            var editableVersionRange = DefaultEditableVersionRange();
            VersionRanges.Add(editableVersionRange);
            SelectedVersionRange = editableVersionRange;
        }

        private static EditableVersionRange DefaultEditableVersionRange()
        {
            var editableVersionRange = new EditableVersionRange();
            var spanishEmptyDocument = new EditableDocument { Language = "es-ES" };
            var englishEmptyDocument = new EditableDocument {Language = "en-GB"};
            editableVersionRange.Documents.Add(spanishEmptyDocument);
            editableVersionRange.Documents.Add(englishEmptyDocument);
            return editableVersionRange;
        }

        private async Task DeleteCurrentVersionRange()
        {
            if (VersionRanges.Count == 1)
            {
                EventBus.Send(new OnUserMessageRequest("No se puede eliminar el único version range de un topic.\nModifique el ya existente o elemine el topic si es lo que deseae"));
                return;
            } 
            VersionRanges.Remove(SelectedVersionRange);
        }

        public VersionRangesVM(EditableTopic editableTopic) : this()
        {
            VersionRanges = editableTopic.VersionRanges;
            SelectedProduct = editableTopic.Product;
        }

        public void Initialize()
        {
            SelectedVersionRange = VersionRanges.ToList().OrderByDescending(vr => vr.ToVersion.Release).First();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DesignModeVersionRangesVM : VersionRangesVM
    {
        public DesignModeVersionRangesVM()
        {
            var anyDocument = new EditableDocument
            {
                Title = "Título",
                Description = "Descripción",
                Content = "Contenido",
                Language = "es-ES"
            };
            var anyEditableVersionRange = new EditableVersionRange
            {
                FromVersion = new ProductVersion("AnyId", "1.0", DateTime.Today),
                ToVersion = new ProductVersion("AnyId", "2.0", DateTime.Today),
            };
            anyEditableVersionRange.Documents.Add(anyDocument);
            var anotherVersionRange = new EditableVersionRange
            {
                FromVersion = new ProductVersion("AnyId", "3.0", DateTime.Today),
                ToVersion = new ProductVersion("AnyId", "4.0", DateTime.Today)
            };
            anotherVersionRange.Documents.Add(anyDocument);
            VersionRanges = new ObservableCollection<EditableVersionRange>
            {
                anyEditableVersionRange,
                anotherVersionRange
            };
        }
    }
}