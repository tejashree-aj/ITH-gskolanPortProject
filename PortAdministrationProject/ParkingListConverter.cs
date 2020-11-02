using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace PortAdministrationProject
{
    [ValueConversion(typeof(List<int>), typeof(string))]
    public class ParkingListConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a String");

            return String.Join(", ", ((List<int>)value).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
