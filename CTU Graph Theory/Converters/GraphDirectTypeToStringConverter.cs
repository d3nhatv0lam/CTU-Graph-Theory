using Avalonia.Data.Converters;
using Avalonia.Media.TextFormatting.Unicode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Converters
{
    public class GraphDirectTypeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string graphDirectName = string.Empty;
            if (value is bool graphDirectType)
            {
                switch (graphDirectType)
                {
                    case false:
                        graphDirectName = "vô hướng";
                        break;
                    case true:
                        graphDirectName = "có hướng";
                        break;
                }
            }
            return graphDirectName;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
