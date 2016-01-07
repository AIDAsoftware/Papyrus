using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Papyrus.Business.Topics;
using Papyrus.Business.VersionRanges;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeToVmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return new VersionRangeVM();
            var versionRange = value as EditableVersionRange;
            return ViewModelsFactory.VersionRange(versionRange);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}