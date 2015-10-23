using System;
using System.ComponentModel;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeVM
    {
        public EditableVersionRange VersionRange { get; set; }

        public VersionRangeVM()
        {
        }

        public VersionRangeVM(EditableVersionRange editableVersionRange)
        {
            VersionRange = editableVersionRange;
        }

        public void Initialize()
        {
        }
    }
}