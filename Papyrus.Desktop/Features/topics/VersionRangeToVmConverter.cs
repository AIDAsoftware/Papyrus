using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangeToVmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return new VersionRangeVM();
            return new VersionRangeVM(value as EditableVersionRange);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}