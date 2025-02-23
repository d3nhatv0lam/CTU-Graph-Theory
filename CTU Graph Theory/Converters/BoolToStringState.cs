using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Converters
{
    public class BoolToStringState : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string state = string.Empty;
            if (value is bool isRunning)
            {
                state += (isRunning) ? "Đang duyệt" : "Không";
            }
            return state;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
