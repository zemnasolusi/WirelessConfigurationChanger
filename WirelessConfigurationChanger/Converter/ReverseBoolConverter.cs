using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WirelessConfigurationChanger.Converter
{
    public class ReverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                bool val = (bool)value;

                if (val)
                    return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                bool val = (bool)value;

                if (val)
                    return false;
            }

            return true;
        }
    }
}
