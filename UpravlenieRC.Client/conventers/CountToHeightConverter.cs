using System;
using System.Globalization;
using System.Windows.Data;

namespace UpravlenieRC.Client.conventers
{
    public class CountToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int count)
                return 0;

            double multiplier = 12;
            if (double.TryParse(parameter?.ToString(), out var m))
                multiplier = m;

            
            return Math.Max(4, count * multiplier);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

