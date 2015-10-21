using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class EditableTopicToVmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return new VersionRangesVM();
            return new VersionRangesVM(value as ObservableCollection<EditableVersionRange>);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}