using System;
using System.Collections.ObjectModel;
using Papyrus.Business.Products;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangesVM
    {
        public ObservableCollection<EditableVersionRange> VersionRanges { get; protected set; }

        public VersionRangesVM()
        {
            VersionRanges = new ObservableCollection<EditableVersionRange>();
        }

        public VersionRangesVM(ObservableCollection<EditableVersionRange> versionRanges)
        {
            VersionRanges = versionRanges;
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