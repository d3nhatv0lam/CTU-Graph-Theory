using Avalonia.Data.Converters;
using CTU_Graph_Theory.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTU_Graph_Theory.Converters
{
    public class EmptyVertexToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string title = value as string;
            if (title == null) return null;
            if (title == Vertex.EmptyVertex.Title) title = "Toàn bộ";
            return title;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
