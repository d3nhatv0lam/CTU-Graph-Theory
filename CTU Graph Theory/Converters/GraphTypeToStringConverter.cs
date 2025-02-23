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
    public class GraphTypeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string graphName = string.Empty;
            if (value is CustomGraph.GraphType graphType) 
            {
                switch (graphType)
                {
                    case CustomGraph.GraphType.SimpleGraph:
                        graphName = "Đơn đồ thị";
                        break;
                    case CustomGraph.GraphType.MultiGraph:
                        graphName = "Đa đồ thị";
                        break;
                    case CustomGraph.GraphType.PseudoGraph:
                        graphName = "Giả đồ thị";
                        break;
                }
            }
            return graphName;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
