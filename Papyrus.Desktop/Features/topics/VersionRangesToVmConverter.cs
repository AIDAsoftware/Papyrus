using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Papyrus.Business.Topics;

namespace Papyrus.Desktop.Features.Topics
{
    public class VersionRangesToVmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
                return new DesignModeVersionRangesVM();
            }

            if (value == null) return new VersionRangesVM();
            return new VersionRangesVM(value as EditableTopic);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}