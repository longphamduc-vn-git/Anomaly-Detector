using System;
using System.Globalization;
using System.Windows.Data;

namespace Anomaly_Detector.Converters
{
    public class IndexToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return index % 2; // Chia vào hai cột (0 và 1)
            }
            return 0; // Mặc định là cột 0 nếu lỗi
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
