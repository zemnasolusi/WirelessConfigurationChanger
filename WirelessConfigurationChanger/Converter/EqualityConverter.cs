using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WirelessConfigurationChanger.Converter
{
    public class EqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.Equals(true))
                return parameter;

            return Binding.DoNothing;
        }
    }
}
